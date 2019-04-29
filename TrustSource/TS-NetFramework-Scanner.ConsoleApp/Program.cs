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
            var trustSourceUserName = app.Option("-user|--username <optionvalue>", "TrustSource Username", CommandOptionType.SingleValue);
            var trustSourceApiKey = app.Option("-key|--ApiKey <optionvalue>", "TrustSource Api Key", CommandOptionType.SingleValue);
            var trustSourceApiUrl = app.Option("-url|--ApiUrl <optionvalue>", "TrustSource Api Url", CommandOptionType.SingleValue);
            var trustSourceBranch = app.Option("-b|--branch <optionvalue>", "TrustSource Branch", CommandOptionType.SingleValue);
            var trustSourceTag = app.Option("-t|--tag <optionvalue>", "TrustSource Tag", CommandOptionType.SingleValue);

            string tsUsername, tsApiKey, tsBranch, tsTag;
            if (trustSourceUserName.HasValue() && trustSourceApiKey.HasValue())
            {
                tsUsername = trustSourceUserName.Value();
                tsApiKey = trustSourceApiKey.Value();
            }
            else
            {
                tsUsername = ConfigurationManager.AppSettings.Get("TS-Username");
                tsApiKey = ConfigurationManager.AppSettings.Get("TS-ApiKey");
            }

            tsBranch = trustSourceBranch.HasValue() ? trustSourceBranch.Value() : projectConfig.Branch;
            tsTag = trustSourceTag.HasValue() ? trustSourceTag.Value() : projectConfig.Tag;

            // When no commands are specified, this block will execute.
            // This is the main "command"

            app.OnExecute(() =>
            {
                if (!string.IsNullOrEmpty(tsUsername) && !string.IsNullOrEmpty(tsApiKey))
                {
                    string projectPath;

                    if (projectPathOption.HasValue())
                    {
                        projectPath = projectPathOption.Value();
                    }
                    else
                    {
                        var configProjectPath = ConfigurationManager.AppSettings.Get("ProjectPath");

                        if (string.IsNullOrEmpty(configProjectPath))
                            projectPath = Environment.CurrentDirectory;
                        else
                            projectPath = configProjectPath;
                    }

                    string apiurl = "";
                    if (trustSourceApiUrl.HasValue())
                    {
                        apiurl = trustSourceApiUrl.Value();
                    }
                    else
                    {
                        var configTsApiUrl = ConfigurationManager.AppSettings.Get("TS-ApiUrl");

                        if (!string.IsNullOrEmpty(configTsApiUrl))
                            apiurl = configTsApiUrl;
                    }

                    Console.WriteLine("Starting Scanning");
                    Console.WriteLine($"Project Path: {projectPath}");
                    Console.WriteLine($"TS Username: {tsUsername}");
                    Console.WriteLine($"TS Api Key: {tsApiKey}");

                    if (!string.IsNullOrEmpty(apiurl))
                        Console.WriteLine($"TS API Url: {apiurl}");

                    TS_NetFramework_Scanner.Engine.Scanner.Initiate(projectPath, tsUsername, tsApiKey, apiurl, tsBranch, tsTag);
                    Console.WriteLine("Scan completed and succefully delivered");
                }
                else
                {
                    Console.WriteLine("TrustSource Username and/or Api Key are not supplied");
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
