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
    public class VSScanner
    {
        public static bool Initiate(string projectPath, string trustSourceUserName, string trustSourceApiKey, string trustSourceApiUrl = "", string tsBranch = "", string tsTag = "")
        {
            MSBuildLocator.RegisterDefaults();
            return DoInitiate(projectPath, trustSourceUserName, trustSourceApiKey, trustSourceApiUrl, tsBranch, tsTag);
        }

        private static bool DoInitiate(string projectPath, string trustSourceUserName, string trustSourceApiKey, string trustSourceApiUrl = "", string tsBranch = "", string tsTag = "")
        {
            string solutionName = VisualStudioProvider.GetSolutionName(projectPath);
            
            SolutionFile solution;
            if (!projectPath.EndsWith(".sln"))
            {
                solution = SolutionFile.Parse($"{projectPath}\\{solutionName}.sln" );
            } else
            {
                solution = SolutionFile.Parse(projectPath);
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
                    if(projectReferences.Count > 0)
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

                // Convert Target into Json string
                string targetJson = TargetSerializer.ConvertToJson(projectTarget);

                // Finally Post Json to Trust Source server
                TrustSourceProvider.PostScan(targetJson, trustSourceUserName, trustSourceApiKey, trustSourceApiUrl);
            }

            return true;
        }
    }
}