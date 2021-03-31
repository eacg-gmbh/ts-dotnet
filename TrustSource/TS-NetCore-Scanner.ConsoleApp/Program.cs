using Microsoft.Extensions.CommandLineUtils;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
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
                    tsApiKey = projectConfig.trustSourceAPI.ApiKey;
                }

                tsApiUrl = trustSourceApiUrl.HasValue() ? trustSourceApiUrl.Value() : projectConfig.trustSourceAPI.ApiUrl;
                tsBranch = trustSourceBranch.HasValue() ? trustSourceBranch.Value() : projectConfig.Branch;
                tsTag = trustSourceTag.HasValue() ? trustSourceTag.Value() : projectConfig.Tag;

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

                    TS_NetCore_Scanner.Engine.Scanner.Initiate(projectPath,tsApiKey, tsApiUrl, tsBranch, tsTag);
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
