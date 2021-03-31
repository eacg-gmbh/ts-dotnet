using NuGet.ProjectModel;
using TS_NetFramework_Scanner.Common;
using TS_NetFramework_Scanner.Engine.NetScanner;

namespace TS_NetFramework_Scanner.Engine
{
    public class Scanner
    {
        public static bool Initiate(string projectPath, string trustSourceApiKey, string trustSourceApiUrl = "", string tsBranch = "", string tsTag = "")
        {
            var dependencyGraphService = new DependencyGraphService();
            var dependencyGraph = dependencyGraphService.GenerateDependencyGraph(projectPath);

            if (dependencyGraph == null)
            {
                return false;
            }

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
                    projectTarget = NetFrameWorkScannerExecutor.ProcessDependencies(solutionName, project, projectPath);
                }

                projectTarget.branch = tsBranch;
                projectTarget.tag = tsTag;

                // Convert Target into Json string
                string targetJson = TargetSerializer.ConvertToJson(projectTarget);

                // Finally Post Json to Trust Source server
                TrustSourceProvider.PostScan(targetJson, trustSourceApiKey, trustSourceApiUrl);
            }

            return true;
        }
    }
}