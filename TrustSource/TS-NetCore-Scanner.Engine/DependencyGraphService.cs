using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using NuGet.ProjectModel;
using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.InteropServices;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TS_NetCore_Scanner.Engine
{
    internal class DependencyGraphService
    {
        public DependencyGraphSpec GenerateDependencyGraph(string projectPath)
        {
            var dotNetRunner = new DotNetRunner("msbuild", !RuntimeInformation.IsOSPlatform(OSPlatform.OSX));
            
            string dgOutput = Path.Combine(Path.GetTempPath(), Path.GetTempFileName());

            string[] arguments = new[] {$"\"{projectPath}\"", "/t:GenerateRestoreGraphFile", $"/p:RestoreGraphOutputPath=\"{dgOutput}\"" };

            RunStatus runStatus;

            try
            {
                runStatus = dotNetRunner.Run(Path.GetDirectoryName(projectPath), arguments);
            }
            catch (Exception ex)
            {
                System.Console.WriteLine("An error occured while executing MSBuild");
                throw ex;
            }

            if (runStatus.IsSuccess)
            {
                string dependencyGraphText = File.ReadAllText(dgOutput);
                return new DependencyGraphSpec(JsonConvert.DeserializeObject<JObject>(dependencyGraphText));
            }
            else
            {
                throw new Exception($"Unable to process the the project `{projectPath}. Are you sure this is a valid .NET Core or .NET Standard project type?" +
                                                     $"\r\n\r\nHere is the full error message returned from the Microsoft Build Engine:\r\n\r\n" + runStatus.Output);
            }
        }
    }
}
