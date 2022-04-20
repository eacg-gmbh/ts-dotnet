using NuGet.ProjectModel;
using System;
using System.Linq;
using TS_Net_Scanner.Common;
using TS_Net_Scanner.Engine;

namespace TS_NetCore_Scanner.Engine
{
    public class Scanner
    {
        public static bool Initiate(string projectPath, string projectName, string trustSourceApiKey, string trustSourceApiUrl = "", string branch = "", string tag = "", bool skipTransfer = false)
        {
            var dependencyGraphService = new DependencyGraphService();
            var dependencyGraph = dependencyGraphService.GenerateDependencyGraph(projectPath);

            string tsProjectName = string.IsNullOrEmpty(projectName) ? VisualStudioProvider.GetSolutionName(projectPath): projectName;

            if (!dependencyGraph.Projects.Any(p => p.RestoreMetadata.ProjectStyle == ProjectStyle.PackageReference))
            {
                throw new Exception($"Unable to process the solution {projectPath}. Are you sure this is a valid .NET Core or .NET Standard project type?");
            }

            foreach (var project in dependencyGraph.Projects.Where(p => p.RestoreMetadata.ProjectStyle == ProjectStyle.PackageReference))
            {
                // Process Project Dependencies
                Target projectTarget = ScannerExcecuter.ProcessDependencies(tsProjectName, project);

                if (!string.IsNullOrEmpty(branch))
                    projectTarget.branch = branch;

                if (!string.IsNullOrEmpty(tag))
                    projectTarget.tag = tag;

                // Convert Target into Json string
                string targetJson = TargetSerializer.ConvertToJson(projectTarget);

                // Finally Post Json to Trust Source server
                if (!skipTransfer)
                {
                    TrustSourceProvider.PostScan(targetJson, trustSourceApiKey, trustSourceApiUrl);
                }
                else
                {
                    Console.WriteLine(targetJson);
                }
                
            }

            return true;
        }
    }
}