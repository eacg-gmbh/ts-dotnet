using Microsoft.Extensions.CommandLineUtils;
using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;

namespace TS_NetCore_Scanner.ConsoleApp
{
    class Program
    {
        static void Main(string[] args)
        {
            var app = new CommandLineApplication();

            // the help text line "Usage: TS-NetCore-Scanne" uses this
            app.Name = "TS-NetCore-Scanner";
            app.Description = ".NET Core project dependency graph and send to TrustSource";
            app.ExtendedHelpText = "Scan .NET Core project dependency graph and send to TrustSource"
                + Environment.NewLine + "Depending on your OS, you may need to execute the application as TS-NetCore-Scanner.exe or 'dotnet TS-NetCore-Scanne.dll'";

            // Set the arguments to display the description and help text
            app.HelpOption("-?|-h|--help");

            // This is a helper/shortcut method to display version info - it is creating a regular Option, with some defaults.
            app.VersionOption("-v|--version", () =>
            {
                return $"Version {Assembly.GetEntryAssembly().GetCustomAttribute<AssemblyInformationalVersionAttribute>().InformationalVersion}";
            });

            var projectPathOption = app.Option("-p|--path <optionvalue>", ".Net Core Project Path", CommandOptionType.SingleValue);
            var trustSourceUserName = app.Option("-user|--username <optionvalue>", "TrustSource Username", CommandOptionType.SingleValue);
            var trustSourceApiKey = app.Option("-key|--ApiKey <optionvalue>", "TrustSource Api Key", CommandOptionType.SingleValue);

            //var trustSourceApiKey = app.Argument("trustSourceApiKey", "TrustSource Api Key for authentication");
            //var trustSourceUserName = app.Argument("trustSourceUsername", "TrustSource username");

            // When no commands are specified, this block will execute.
            // This is the main "command"
            app.OnExecute(() =>
            {
                if (trustSourceUserName.HasValue() && trustSourceApiKey.HasValue())
                {
                    string projectPath;

                    if (projectPathOption.HasValue())
                    {
                        projectPath = projectPathOption.Value();
                    }
                    else
                    {
                        projectPath = Environment.CurrentDirectory;
                    }

                    //Console.WriteLine(trustSourceUserName.Value);
                    //Console.WriteLine(trustSourceApiKey.Value);
                    //Console.WriteLine(projectPath);

                    Console.WriteLine("Starting Scanning");
                    TS_NetCore_Scanner.Engine.Scanner.Initiate(projectPath, trustSourceUserName.Value(), trustSourceApiKey.Value());
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
