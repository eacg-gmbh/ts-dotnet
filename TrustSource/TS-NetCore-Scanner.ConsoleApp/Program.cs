using Microsoft.Extensions.CommandLineUtils;
using Microsoft.Extensions.Configuration;
using System;
using System.Reflection;

namespace TS_NetCore_Scanner.ConsoleApp
{
    class Program
    {
        static void Main(string[] args)
        {
            var builder = new ConfigurationBuilder()
                //.SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true);

            IConfigurationRoot configuration = builder.Build();

            ProjectConfig projectConfig = new ProjectConfig();
            configuration.Bind(projectConfig);

            var app = new CommandLineApplication();

            // the help text line "Usage: TS-NetCore-Scanne" uses this
            app.Name = "TS-NetCore-Scanner";
            app.Description = ".NET Core project dependency graph and send to TrustSource";
            app.ExtendedHelpText = "Scan .NET Core project dependency graph and send to TrustSource"
                + Environment.NewLine + "you may need to execute the application as 'dotnet TS-NetCore-Scanne.dll'";

            // Set the arguments to display the description and help text
            app.HelpOption("-?|-h|--help");

            // This is a helper/shortcut method to display version info - it is creating a regular Option, with some defaults.
            app.VersionOption("-v|--version", () =>
            {
                return $"Version {Assembly.GetEntryAssembly().GetCustomAttribute<AssemblyInformationalVersionAttribute>().InformationalVersion}";
            });

            var projectPathOption = app.Option("-p|--path <optionvalue>", ".Net Core Project Path", CommandOptionType.SingleValue);            
            var trustSourceApiKey = app.Option("-key|--ApiKey <optionvalue>", "TrustSource Api Key", CommandOptionType.SingleValue);
            var trustSourceApiUrl = app.Option("-url|--ApiUrl <optionvalue>", "TrustSource Api Url", CommandOptionType.SingleValue);
            var trustSourceBranch = app.Option("-b|--branch <optionvalue>", "TrustSource Branch", CommandOptionType.SingleValue);
            var trustSourceTag = app.Option("-t|--tag <optionvalue>", "TrustSource Tag", CommandOptionType.SingleValue);
            var projectName = app.Option("--projectName <optionvalue>", "TrustSource Project Name", CommandOptionType.SingleValue);
            var moduleName = app.Option("--moduleName <optionvalue>", "TrustSource Module Name (when --solutionAsModule is on)", CommandOptionType.SingleValue);
            var skipTransfer = app.Option("--skipTransfer", "Skip transfer of scan results to the TrustSource service", CommandOptionType.NoValue);
            var solutionAsModule = app.Option("--solutionAsModule", "Process the VS solution as one module", CommandOptionType.NoValue);
            

            // When no commands are specified, this block will execute.
            // This is the main "command"

            app.OnExecute(() =>
            {
                // Params
                string tsApiKey = trustSourceApiKey.HasValue() ? trustSourceApiKey.Value(): projectConfig.trustSourceAPI.ApiKey;
                string tsApiUrl = trustSourceApiUrl.HasValue() ? trustSourceApiUrl.Value() : projectConfig.trustSourceAPI.ApiUrl;
                string tsBranch = trustSourceBranch.HasValue() ? trustSourceBranch.Value() : projectConfig.Branch;
                string tsTag = trustSourceTag.HasValue() ? trustSourceTag.Value() : projectConfig.Tag;
                string tsProjectName = projectName.HasValue() ? projectName.Value() : "";
                string tsModuleName = moduleName.HasValue() ? moduleName.Value() : "";

                // Flags
                bool tsSkipTransfer = skipTransfer.HasValue();
                bool tsSolutionAsModule = solutionAsModule.HasValue();
                
                if (!string.IsNullOrEmpty(tsApiKey))
                {
                    string projectPath = projectPathOption.HasValue() ? projectPathOption.Value() : Environment.CurrentDirectory;

                    Console.WriteLine("Starting Scanning");
                    Console.WriteLine($"Project Path: {projectPath}");
                    Console.WriteLine($"TS Api Key: {tsApiKey}");

                    if (!string.IsNullOrEmpty(tsBranch))
                        Console.WriteLine($"TS Branch: {tsBranch}");

                    if (!string.IsNullOrEmpty(tsTag))
                        Console.WriteLine($"TS Tag: {tsTag}");

                    if (!string.IsNullOrEmpty(tsApiUrl))
                        Console.WriteLine($"TS API Url: {tsApiUrl}");

                    Engine.Scanner.Initiate(projectPath, tsProjectName, tsModuleName, tsApiKey, tsApiUrl, tsBranch, tsTag, tsSkipTransfer, tsSolutionAsModule);
                }
                else
                {
                    Console.WriteLine("TrustSource Username and/or Api Key are not supplied");                    
                    app.ShowHint();
                }

                return 0;
            });


            try
            {
                // This begins the actual execution of the application
                Console.WriteLine("TS-NetCore-Scanner is in progress...");
                app.Execute(args);
                Console.WriteLine("TS-NetCore-Scanner process has been completed");
            }
            catch (CommandParsingException ex)
            {
                // Catch exceptions
                Console.WriteLine("Unable to complete scan: {0}", ex.Message);
            }
            catch (Exception ex)
            {
                Console.WriteLine("Unable to execute application: {0}", ex.Message);
            }
        }
    }
}
