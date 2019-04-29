using NuGet.ProjectModel;
using System;
using System.Linq;
using TS_Net_Scanner.Common;
using TS_Net_Scanner.Engine;

namespace TS_NetCore_Scanner.Engine
{
    public class Scanner
    {
        public static bool Initiate(string projectPath, string trustSourceUserName, string trustSourceApiKey, string trustSourceApiUrl = "", string branch = "", string tag = "")
        {
            var dependencyGraphService = new DependencyGraphService();
            var dependencyGraph = dependencyGraphService.GenerateDependencyGraph(projectPath);
            string solutionName = VisualStudioProvider.GetSolutionName(projectPath);

            if (!dependencyGraph.Projects.Any(p => p.RestoreMetadata.ProjectStyle == ProjectStyle.PackageReference))
            {
                throw new Exception($"Unable to process the solution {projectPath}. Are you sure this is a valid .NET Core or .NET Standard project type?");
            }

            foreach (var project in dependencyGraph.Projects.Where(p => p.RestoreMetadata.ProjectStyle == ProjectStyle.PackageReference))
            {
                // Process Project Dependencies
                Target projectTarget = ScannerExcecuter.ProcessDependencies(solutionName, project);

                if (!string.IsNullOrEmpty(branch))
                    projectTarget.branch = branch;

                if (!string.IsNullOrEmpty(tag))
                    projectTarget.tag = tag;

                // Convert Target into Json string
                string targetJson = TargetSerializer.ConvertToJson(projectTarget);

                // Finally Post Json to Trust Source server
                TrustSourceProvider.PostScan(targetJson, trustSourceUserName, trustSourceApiKey, trustSourceApiUrl);
            }

            return true;
        }
    }
}