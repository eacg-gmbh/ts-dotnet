using Microsoft.Extensions.CommandLineUtils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Configuration;

namespace TS_NetFramework_Scanner.ConsoleApp
{
    class Program
    {
        static void Main(string[] args)
        {
            var app = new CommandLineApplication();

            // the help text line "Usage: TS-NetFramework-Scanner" uses this
            app.Name = "TS-NetFramework-Scanner";
            app.Description = ".NET and .NET Core project dependency graph and send to TrustSource";
            app.ExtendedHelpText = "Scan .NET and .NET Core project dependency graph and send to TrustSource"
                + Environment.NewLine + "you may need to execute the application as 'dotnet TS-NetFramework-Scanne.dll'";

            // Set the arguments to display the description and help text
            app.HelpOption("-?|-h|--help");

            // This is a helper/shortcut method to display version info - it is creating a regular Option, with some defaults.
            app.VersionOption("-v|--version", () =>
            {
                return $"Version {Assembly.GetEntryAssembly().GetCustomAttribute<AssemblyInformationalVersionAttribute>().InformationalVersion}";
            });

            var projectPathOption = app.Option("-p|--path <optionvalue>", ".Net Project Path", CommandOptionType.SingleValue);
            var trustSourceApiKey = app.Option("-key|--ApiKey <optionvalue>", "TrustSource Api Key", CommandOptionType.SingleValue);
            var trustSourceApiUrl = app.Option("-url|--ApiUrl <optionvalue>", "TrustSource Api Url", CommandOptionType.SingleValue);
            var trustSourceBranch = app.Option("-b|--branch <optionvalue>", "TrustSource Branch", CommandOptionType.SingleValue);
            var trustSourceTag = app.Option("-t|--tag <optionvalue>", "TrustSource Tag", CommandOptionType.SingleValue);

 
            // When no commands are specified, this block will execute.
            // This is the main "command"

            app.OnExecute(() =>
            {
                string tsApiKey, tsApiUrl, tsBranch, tsTag;
                if (trustSourceApiKey.HasValue())
                {                   
                    tsApiKey = trustSourceApiKey.Value();
                }
                else
                {                   
                    tsApiKey = ConfigurationManager.AppSettings.Get("TS-ApiKey");
                }

                tsApiUrl = trustSourceApiUrl.HasValue() ? trustSourceApiUrl.Value() : ConfigurationManager.AppSettings.Get("TS-ApiUrl");
                tsBranch = trustSourceBranch.HasValue() ? trustSourceBranch.Value() :ConfigurationManager.AppSettings.Get("TS-Branch");
                tsTag = trustSourceTag.HasValue() ? trustSourceTag.Value() : ConfigurationManager.AppSettings.Get("TS-Tag");

                if (!string.IsNullOrEmpty(tsApiKey))
                {
                    string projectPath = projectPathOption.HasValue() ? projectPathOption.Value() : ConfigurationManager.AppSettings.Get("ProjectPath");

                    if (string.IsNullOrEmpty(projectPath))
                    {
                        projectPath = Environment.CurrentDirectory;
                    }
                        
                    Console.WriteLine("Starting Scanning");
                    Console.WriteLine($"Project Path: {projectPath}");                   
                    Console.WriteLine($"TS Api Key: {tsApiKey}");

                    if (!string.IsNullOrEmpty(tsBranch))
                        Console.WriteLine($"TS Branch: {tsBranch}");

                    if (!string.IsNullOrEmpty(tsTag))
                        Console.WriteLine($"TS Tag: {tsTag}");

                    if (!string.IsNullOrEmpty(tsApiUrl))
                        Console.WriteLine($"TS API Url: {tsApiUrl}");

                    try {
                        TS_NetFramework_Scanner.Engine.Scanner.Initiate(projectPath, tsApiKey, tsApiUrl, tsBranch, tsTag);
                    } catch (Exception ex)
                    {
                        Console.WriteLine("Error occured while scanning NuGet dependencies: {0}", ex.Message);
                    }

                    try
                    {
                        TS_NetFramework_Scanner.Engine.VSScanner.LocateMSBuild();
                        TS_NetFramework_Scanner.Engine.VSScanner.Initiate(projectPath, tsApiKey, tsApiUrl, tsBranch, tsTag);
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine("Error occured while scanning VS solution dependencies: {0}", ex.Message);
                    }

                    Console.WriteLine("Scan completed and succefully delivered");
                }
                else
                {
                    Console.WriteLine("TrustSource API Key is not supplied");
                    // ShowHint() will display: "Specify --help for a list of available options and commands."
                    app.ShowHint();
                }

                return 0;
            });

            try
            {
                // This begins the actual execution of the application
                Console.WriteLine("TS-NetFramework-Scanner is in progress...");
                app.Execute(args);
                Console.WriteLine("TS-NetFramework-Scanner process has been completed");
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
