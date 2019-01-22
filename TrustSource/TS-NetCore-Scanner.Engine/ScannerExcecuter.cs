using NuGet.ProjectModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TS_NetCore_Scanner.Engine
{
    internal class ScannerExcecuter
    {
        internal static Target ProcessDependencies(PackageSpec project)
        {
            Target projectTarget = new Target();
            projectTarget.project = project.Name;
            projectTarget.moduleId = $"netcore:{project.Name}";
            projectTarget.module = project.Name;
            projectTarget.release = project.Version.ToFullString();

            projectTarget.dependencies = new List<Dependency>();

            // Generate lock file
            var lockFileService = new LockFileService();
            var lockFile = lockFileService.GetLockFile(project.FilePath, project.RestoreMetadata.OutputPath);

            foreach (var targetFramework in project.TargetFrameworks)
            {
                var lockFileTargetFramework = lockFile.Targets.FirstOrDefault(t => t.TargetFramework.Equals(targetFramework.FrameworkName));
                if (lockFileTargetFramework != null)
                {
                    foreach (var dependency in targetFramework.Dependencies)
                    {
                        var projectLibrary = lockFileTargetFramework.Libraries.FirstOrDefault(library => library.Name == dependency.Name);
                        ReportDependency(projectTarget.dependencies, projectLibrary, lockFileTargetFramework);
                    }
                }
            }

            return projectTarget;
        }

        private static void ReportDependency(List<Dependency> dependencies, LockFileTargetLibrary projectLibrary, LockFileTarget lockFileTargetFramework)
        {
            Dependency targetDependency = new Dependency();
            dependencies.Add(targetDependency);

            targetDependency.name = projectLibrary.Name; //targetFramework.FrameworkName.ToString(); // $"{targetFramework.FrameworkName.DotNetFrameworkName} {targetFramework.FrameworkName.Version}";
            targetDependency.key = $"netcore:{projectLibrary.Name}";
            targetDependency.versions.Add(projectLibrary.Version.ToNormalizedString());

            //targetDependency.checksum = "";
            //targetDependency.homepageUrl = "";
            //targetDependency.repoUrl = "";
            //targetDependency.description = projectLibrary.Version.ToFullString();
            //targetDependency.licences.Add(new licence() { name = projectLibrary.Version.ToFullString(), url = "" });

            foreach (var childDependency in projectLibrary.Dependencies)
            {
                var childLibrary = lockFileTargetFramework.Libraries.FirstOrDefault(library => library.Name == childDependency.Id);
                ReportDependency(targetDependency.dependencies, childLibrary, lockFileTargetFramework);
            }
        }
    }
}
