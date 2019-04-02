using NuGet.ProjectModel;
using TS_Net_Scanner.Engine;
using TS_NetFramework_Scanner.Common;
using TS_NetFramework_Scanner.Engine.NetScanner;

namespace TS_NetFramework_Scanner.Engine
{
    public class Scanner
    {
        public static bool Initiate(string projectPath, string trustSourceUserName, string trustSourceApiKey, string trustSourceApiUrl = "")
        {
            var dependencyGraphService = new DependencyGraphService();
            var dependencyGraph = dependencyGraphService.GenerateDependencyGraph(projectPath);
            string solutionName = VisualStudioProvider.GetSolutionName(projectPath);

            foreach (var project in dependencyGraph.Projects)
            {
                Target projectTarget = null;

                if (project.RestoreMetadata.ProjectStyle == ProjectStyle.PackageReference)
                {
                    // Process Project Dependencies
                    projectTarget = NetCoreScannerExcecuter.ProcessDependencies(solutionName, project);
                }
                else
                {
                    projectTarget = NetFrameWorkScannerExecutor.ProcessDependencies(solutionName, project);
                }
                
                // Convert Target into Json string
                string targetJson = TargetSerializer.ConvertToJson(projectTarget);

                // Finally Post Json to Trust Source server
                TrustSourceProvider.PostScan(targetJson, trustSourceUserName, trustSourceApiKey, trustSourceApiUrl);
            }

            return true;
        }
    }
}