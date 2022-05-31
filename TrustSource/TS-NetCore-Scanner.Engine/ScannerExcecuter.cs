using NuGet.ProjectModel;
using System.Collections.Generic;
using System.Linq;
using TS_Net_Scanner.Common;
using TS_Net_Scanner.Engine;

namespace TS_NetCore_Scanner.Engine
{
    internal static class LockFileTargetExtension {
        internal static LockFileTargetLibrary FindLibrary(this LockFileTarget target, string id)
        {
            var library = target.Libraries.FirstOrDefault(lib => lib.Name == id);
            if (library == null)
            {
                System.Console.WriteLine($"WARNING: Cannot find library for {id} in assets");
            }
            return library;
        }
    }

    internal class ScannerExcecuter
    {
        internal static Target ProcessDependencies(string tsProjectName, PackageSpec project)
        {
            List<string> packageCollection = new List<string>();

            Target projectTarget = new Target();
            projectTarget.project = tsProjectName;
            projectTarget.moduleId = $"nuget:{project.Name}";
            projectTarget.module = project.Name;
            projectTarget.release = project.Version.ToFullString();

            projectTarget.dependencies = new List<Dependency>();

            // Generate lock file
            var lockFileService = new LockFileService();
            var lockFile = lockFileService.GetLockFile(project.FilePath, project.RestoreMetadata.OutputPath);

            if (lockFile == null) {
                System.Console.WriteLine($"WARNING: Cannot restore lock file for the project {project.Name}.");
                return projectTarget;
            }

            foreach (var targetFramework in project.TargetFrameworks)
            {
                var lockFileTargetFramework = lockFile.Targets.FirstOrDefault(t => t.TargetFramework.Equals(targetFramework.FrameworkName));
                if (lockFileTargetFramework != null)
                {
                    foreach (var dependency in targetFramework.Dependencies)
                    {                        
                        var projectLibrary = lockFileTargetFramework.FindLibrary(dependency.Name);
                        if (projectLibrary != null)
                        {
                            ReportDependency(projectTarget.dependencies, projectLibrary, lockFileTargetFramework, dependency.AutoReferenced, packageCollection);
                        }                        
                    }
                }
            }

            return projectTarget;
        }

        private static void ReportDependency(List<Dependency> dependencies, LockFileTargetLibrary projectLibrary, LockFileTarget lockFileTargetFramework, bool AutoReferenced, List<string> packageCollection)
        {
            Dependency targetDependency = new Dependency();
            dependencies.Add(targetDependency);

            targetDependency.name = projectLibrary.Name;
            targetDependency.key = $"nuget:{projectLibrary.Name}";
            targetDependency.versions.Add(projectLibrary.Version.ToNormalizedString());

            //targetDependency.checksum = "";
            //targetDependency.homepageUrl = "";
            //targetDependency.repoUrl = "";
            //targetDependency.description = projectLibrary.Version.ToFullString();
            //targetDependency.licences.Add(new licence() { name = projectLibrary.Version.ToFullString(), url = "" });

            if (!AutoReferenced)
            {
                packageCollection.Add(projectLibrary.Name);

                foreach (var childDependency in projectLibrary.Dependencies)
                {
                    var childLibrary = lockFileTargetFramework.FindLibrary(childDependency.Id);
                    if (childLibrary != null)
                    {
                        bool SystemReferenced = MetaPackagesSkipper.MetaPackages.Any(x => x == childLibrary.Name) || packageCollection.Any(x => x == childLibrary.Name);
                        ReportDependency(targetDependency.dependencies, childLibrary, lockFileTargetFramework, SystemReferenced, packageCollection);
                    }
                }
            }
        }
    }
}
