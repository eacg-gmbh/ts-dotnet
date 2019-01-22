using NuGet.ProjectModel;
using System;
using System.Linq;

namespace TS_NetCore_Scanner.Engine
{
    public class Scanner
    {
        public static bool Initiate(string projectPath, string trustSourceUserName, string trustSourceApiKey)
        {
            var dependencyGraphService = new DependencyGraphService();
            var dependencyGraph = dependencyGraphService.GenerateDependencyGraph(projectPath);

            if (!dependencyGraph.Projects.Any(p => p.RestoreMetadata.ProjectStyle == ProjectStyle.PackageReference))
            {
                throw new Exception($"Unable to process the solution {projectPath}. Are you sure this is a valid .NET Core or .NET Standard project type?");
            }

            foreach (var project in dependencyGraph.Projects.Where(p => p.RestoreMetadata.ProjectStyle == ProjectStyle.PackageReference))
            {
                // Process Project Dependencies
                Target projectTarget = ScannerExcecuter.ProcessDependencies(project);

                // Convert Target into Json string
                string targetJson = TargetSerializer.ConvertToJson(projectTarget);

                // Finally Post Json to Trust Source server
                TrustSourceProvider.PostScan(targetJson, trustSourceUserName, trustSourceApiKey);
            }

            return true;
        }
    }
}