using System.Collections.Generic;
using NuGet.ProjectModel;
using TS_NetFramework_Scanner.Common;
using TS_NetFramework_Scanner.Engine.NetScanner;

namespace TS_NetFramework_Scanner.Engine
{
    class NuGetScanner
    {
        public static List<Target> Execute(string projectPath, string tsBranch, string tsTag)
        {
            var targets = new List<Target>();

            var dependencyGraphService = new DependencyGraphService();
            var dependencyGraph = dependencyGraphService.GenerateDependencyGraph(projectPath);

            if (dependencyGraph != null)
            {
                string solutionName = VisualStudioProvider.GetSolutionName(projectPath);

                foreach (var project in dependencyGraph.Projects)
                {
                    Target projectTarget;

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

                    targets.Add(projectTarget);
                }
            }
            
            return targets;
        }
    }
}