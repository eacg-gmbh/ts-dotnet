using Microsoft.Build.Locator;
using Microsoft.Build.Construction;
using Microsoft.Build.Evaluation;

using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;

using TS_NetFramework_Scanner.Common;
using TS_NetFramework_Scanner.Engine.NetScanner;

namespace TS_NetFramework_Scanner.Engine
{
    class VSScanner
    {
        public static void LocateMSBuild()
        {
            MSBuildLocator.RegisterDefaults();
        }

        public static List<Target> Execute(string projectPath, string tsBranch, string tsTag)
        {
            var targets = new List<Target>();

            string solutionName = VisualStudioProvider.GetSolutionName(projectPath);
            string solutionPath = VisualStudioProvider.GetSolutionPath(projectPath);

            if (String.IsNullOrWhiteSpace(solutionPath))
            {
                throw new Exception("No VS solution path is provided.");
            }

            var solution = SolutionFile.Parse(solutionPath);
            if (solution == null)
            {
                throw new Exception("Cannot load VS solution.");
            }

            var projectCollection = new ProjectCollection();

            foreach (var project in solution.ProjectsInOrder)
            {
                Target projectTarget = new Target();

                projectTarget.project = solutionName;
                projectTarget.moduleId = $"vs:{project.ProjectName}";
                projectTarget.module = project.ProjectName;
                projectTarget.release = "0.0.0";

                projectTarget.dependencies = new List<Dependency>();
         
                var projectDetails = projectCollection.LoadProject(project.AbsolutePath);           
                if (projectDetails != null)
                {                    
                    var projectReferences = projectDetails.GetItems("ProjectReference");
                    var projectTargetFramework = projectDetails.GetPropertyValue("TargetRuntime");

                    if (projectReferences.Count > 0)
                    {
                        foreach (var projRef in projectReferences)
                        {
                            var projRefGuid = projRef.GetMetadataValue("Project");
                            if(!String.IsNullOrEmpty(projRefGuid))
                            {
                                var slnProjRef = solution.ProjectsByGuid[projRefGuid];
                                if (slnProjRef != null)
                                {
                                    Dependency targetDependency = new Dependency();
                                    projectTarget.dependencies.Add(targetDependency);

                                    targetDependency.name = slnProjRef.ProjectName;
                                    targetDependency.key = $"vs:{slnProjRef.ProjectName}";
                                    targetDependency.versions.Add("0.0.0");
                                }
                            }
                        }
                    }
                    else
                    {
                        Dependency targetDependency = new Dependency();
                        projectTarget.dependencies.Add(targetDependency);

                        targetDependency.key = $"vs:{project.ProjectName}";
                        targetDependency.name = project.ProjectName;                       
                        targetDependency.versions.Add("0.0.0");
                    }
                }

                if (!string.IsNullOrEmpty(tsBranch))
                    projectTarget.branch = tsBranch;

                if (!string.IsNullOrEmpty(tsTag))
                    projectTarget.tag = tsTag;

                targets.Add(projectTarget);
            }

            return targets;
        }
    }
}