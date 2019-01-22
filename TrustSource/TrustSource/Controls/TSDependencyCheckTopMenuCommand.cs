using System;
using System.ComponentModel.Design;
using System.Globalization;
using System.Threading;
using System.Threading.Tasks;
using EnvDTE;
using EnvDTE80;
using Microsoft;
using Microsoft.VisualStudio.Shell;
using Microsoft.VisualStudio.Shell.Interop;
using TrustSource.Common;
using TrustSource.Models;
using TS_NetCore_Scanner.Engine;
using Task = System.Threading.Tasks.Task;

namespace TrustSource
{
    /// <summary>
    /// Command handler
    /// </summary>
    internal sealed class TSDependencyCheckTopMenuCommand
    {
        /// <summary>
        /// Command ID.
        /// </summary>
        public const int CommandId = 0x0100;

        /// <summary>
        /// Command menu group (command set GUID).
        /// </summary>
        public static readonly Guid CommandSet = new Guid("7d57f524-a5ef-4378-945c-c3f79c156dc9");

        /// <summary>
        /// VS Package that provides this command, not null.
        /// </summary>
        private readonly AsyncPackage package;

        /// <summary>
        /// Initializes a new instance of the <see cref="TSDependencyCheckTopMenuCommand"/> class.
        /// Adds our command handlers for menu (commands must exist in the command table file)
        /// </summary>
        /// <param name="package">Owner package, not null.</param>
        /// <param name="commandService">Command service to add command to, not null.</param>
        private TSDependencyCheckTopMenuCommand(AsyncPackage package, OleMenuCommandService commandService)
        {
            this.package = package ?? throw new ArgumentNullException(nameof(package));
            commandService = commandService ?? throw new ArgumentNullException(nameof(commandService));

            var menuCommandID = new CommandID(CommandSet, CommandId);
            var menuItem = new MenuCommand(ExecuteAsync, menuCommandID);
            commandService.AddCommand(menuItem);
        }

        /// <summary>
        /// Gets the instance of the command.
        /// </summary>
        public static TSDependencyCheckTopMenuCommand Instance
        {
            get;
            private set;
        }

        /// <summary>
        /// Gets the service provider from the owner package.
        /// </summary>
        private Microsoft.VisualStudio.Shell.IAsyncServiceProvider ServiceProvider
        {
            get
            {
                return this.package;
            }
        }

        /// <summary>
        /// Initializes the singleton instance of the command.
        /// </summary>
        /// <param name="package">Owner package, not null.</param>
        public static async Task InitializeAsync(AsyncPackage package)
        {
            // Switch to the main thread - the call to AddCommand in TSDependencyCheckTopMenuCommand's constructor requires
            // the UI thread.
            await ThreadHelper.JoinableTaskFactory.SwitchToMainThreadAsync(package.DisposalToken);

            OleMenuCommandService commandService = await package.GetServiceAsync((typeof(IMenuCommandService))) as OleMenuCommandService;
            Instance = new TSDependencyCheckTopMenuCommand(package, commandService);
        }

        /// <summary>
        /// This function is the callback used to execute the command when the menu item is clicked.
        /// See the constructor to see how the menu item is associated with this function using
        /// OleMenuCommandService service and MenuCommand class.
        /// </summary>
        /// <param name="sender">Event sender.</param>
        /// <param name="e">Event args.</param>
        private async void ExecuteAsync(object sender, EventArgs e)
        {
            await ThreadHelper.JoinableTaskFactory.SwitchToMainThreadAsync();

            string title = "TrustSource Scanner";

            TrustSourceSettings tsSettings = ((TrustSourceToolsOptions)package).TrustSourceApiSettings;
            bool IsApiConfigured = !(tsSettings == null || string.IsNullOrEmpty(tsSettings.ApiKey) || string.IsNullOrEmpty(tsSettings.Username));  

            if(!IsApiConfigured)
            {
                string message = "TrustSource Api credentials are not avialable. Please go to Tools preferences and set TrustSource credentails.";

                VsShellUtilities.ShowMessageBox(
                    this.package,
                    message,
                    title,
                    OLEMSGICON.OLEMSGICON_INFO,
                    OLEMSGBUTTON.OLEMSGBUTTON_OK,
                    OLEMSGDEFBUTTON.OLEMSGDEFBUTTON_FIRST);

                return;
            }

            DTE dte = (DTE)await ServiceProvider.GetServiceAsync(typeof(DTE));

            if (dte != null && !dte.Application.Solution.IsOpen)
            {
                string message = "There is no solution open. Please first open a solution and try again.";

                VsShellUtilities.ShowMessageBox(
                    this.package,
                    message,
                    title,
                    OLEMSGICON.OLEMSGICON_INFO,
                    OLEMSGBUTTON.OLEMSGBUTTON_OK,
                    OLEMSGDEFBUTTON.OLEMSGDEFBUTTON_FIRST);

                return;
            }

            IVsStatusbar statusBar = (IVsStatusbar)await ServiceProvider.GetServiceAsync(typeof(SVsStatusbar));

            if(statusBar != null)
            {
                // Make sure the status bar is not frozen  
                int frozen;

                statusBar.IsFrozen(out frozen);

                if (frozen != 0)
                {
                    statusBar.FreezeOutput(0);
                }

                statusBar.SetText("TrustSource Scanner is in progress..");
                statusBar.FreezeOutput(1);
            }

            try
            {
                SolutionBuild builder = dte.Application.Solution.SolutionBuild;
                SolutionConfiguration2 config = (SolutionConfiguration2)builder.ActiveConfiguration;

                var activeProject = Helper.GetActiveProject(dte);
                string projectPath = activeProject.FullName;

                Scanner.Initiate(projectPath, tsSettings.Username, tsSettings.ApiKey);


                // Get the status bar text.   
                string text;
                statusBar.GetText(out text);
                System.Windows.Forms.MessageBox.Show(text);

                statusBar.SetText("TrustSource Scan is completed");
                statusBar.FreezeOutput(1);
                statusBar.Clear();

                System.Windows.Forms.MessageBox.Show("TrustSource scan is completed");
            }
            catch (Exception ex)
            {
                // Show a message box to prove we were here
                VsShellUtilities.ShowMessageBox(
                    this.package,
                    ex.Message,
                    title,
                    OLEMSGICON.OLEMSGICON_CRITICAL,
                    OLEMSGBUTTON.OLEMSGBUTTON_OK,
                    OLEMSGDEFBUTTON.OLEMSGDEFBUTTON_FIRST);

                statusBar.SetText("TrustSource Scan: Something went wrong");
                statusBar.FreezeOutput(0);
                statusBar.Clear();

                return;
            }
        }
    }
}
