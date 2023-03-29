using NuGet.ProjectModel;
using System;
using System.Linq;
using System.Collections.Generic;
using TS_Net_Scanner.Common;
using TS_Net_Scanner.Engine;

namespace TS_NetCore_Scanner.Engine
{
    public class Scanner
    {
        public static bool Initiate(string projectPath,
                                    string projectName,
                                    string moduleName,
                                    string trustSourceApiKey,
                                    string trustSourceApiUrl = "",
                                    string branch = "",
                                    string tag = "",
                                    bool skipTransfer = false,
                                    bool solutionAsModule = false)
        {
            var dependencyGraphService = new DependencyGraphService();
            var dependencyGraph = dependencyGraphService.GenerateDependencyGraph(projectPath);

            string tsProjectName = string.IsNullOrEmpty(projectName) ? VisualStudioProvider.GetSolutionName(projectPath): projectName;
            string tsModuleName = string.IsNullOrEmpty(moduleName) ? VisualStudioProvider.GetSolutionName(projectPath) : moduleName;

            if (!dependencyGraph.Projects.Any(p => p.RestoreMetadata.ProjectStyle == ProjectStyle.PackageReference))
            {
                throw new Exception($"Unable to process the solution {projectPath}. Are you sure this is a valid .NET Core or .NET Standard project type?");
            }


            void processTarget(Target target)
            {
                if (!string.IsNullOrEmpty(branch))
                    target.branch = branch;

                if (!string.IsNullOrEmpty(tag))
                    target.tag = tag;

                // Convert Target into Json string
                string targetJson = TargetSerializer.ConvertToJson(target);

                // Finally Post Json to Trust Source server
                if (!skipTransfer)
                {
                    var response = TrustSourceProvider.PostScan(targetJson, trustSourceApiKey, trustSourceApiUrl);
                    Console.WriteLine(response);
                }
                else
                {
                    Console.WriteLine(targetJson);
                }
            }


            if (solutionAsModule) {
                ProcessDependencies(dependencyGraph, tsProjectName, tsModuleName, processTarget);                
            } else {
                ProcessDependencies(dependencyGraph, tsProjectName, processTarget);
            }
            
            return true;
        }


        private static void ProcessDependencies(DependencyGraphSpec dependencyGraph, string projectName, Action<Target> processTarget) {
            foreach (var project in dependencyGraph.Projects.Where(p => p.RestoreMetadata.ProjectStyle == ProjectStyle.PackageReference))
            {                                
                processTarget(ScannerExcecuter.ProcessDependencies(projectName, project));
            }
        }

        private static void ProcessDependencies(DependencyGraphSpec dependencyGraph, string projectName, string moduleName, Action<Target> processTarget)
        {
            Target projectTarget = new Target();
            projectTarget.project = projectName;
            projectTarget.moduleId = $"nuget:{moduleName}";
            projectTarget.module = moduleName;

            projectTarget.dependencies = new List<Dependency>();
           
            ProcessDependencies(dependencyGraph, projectName, (Target t) => {
                projectTarget.dependencies.AddRange(t.dependencies);
            });

            processTarget(projectTarget);
        }
    }
}