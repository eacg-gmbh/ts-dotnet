using NuGet;
using NuGet.ProjectModel;
using System.Collections.Generic;
using System.Linq;
using TS_Net_Scanner.Common;

namespace TS_Net_Scanner.Engine.NetScanner
{
    internal class NetFrameWorkScannerExecutor
    {
        internal static Target ProcessDependencies(string solutionName, PackageSpec project)
        {
            List<string> packageCollection = new List<string>();

            Target projectTarget = new Target();
            projectTarget.project = solutionName;
            projectTarget.moduleId = $"netframework:{project.Name}";
            projectTarget.module = project.Name;
            projectTarget.release = project.Version.ToFullString();

            projectTarget.dependencies = new List<Dependency>();

            // Generate lock file
            var lockFileService = new LockFileService();
            var lockFile = lockFileService.GetLockFile(project.FilePath, project.RestoreMetadata.OutputPath);

            foreach (var targetFramework in project.TargetFrameworks)
            {
                var lockFileTargetFramework = lockFile.Targets.FirstOrDefault(t => t.TargetFramework.Equals(targetFramework.FrameworkName));
                ReportDependency(projectTarget.dependencies, targetFramework, false, packageCollection, lockFileTargetFramework);
            }

            var packageRepository = new NuGet.LocalPackageRepository($@"{project.BaseDirectory}\packages");
            IQueryable<IPackage> packages = packageRepository.GetPackages();

            foreach (IPackage package in packages)
            {
                ReportDependencyPackage(projectTarget.dependencies, package, true, packageCollection);
            }

            return projectTarget;
        }

        private static void ReportDependencyPackage(List<Dependency> dependencies, IPackage package, bool AutoReferenced, List<string> packageCollection)
        {
            Dependency targetDependency = new Dependency();
            dependencies.Add(targetDependency);

            targetDependency.key = "nuget:" + package.Id;
            targetDependency.name = package.Id;

            //versions = new List<string> { package.Version.ToString() },

            //targetDependency.description = package.Description,
            //homepageUrl = package.ProjectUrl?.AbsoluteUri,

            //targetDependency.name = projectLibrary.FrameworkName.ToString();
            //targetDependency.key = $"netframework:{projectLibrary.FrameworkName.ToString()}";

            if (!AutoReferenced)
            {
                packageCollection.Add(package.GetFullName());

                foreach (var dependencySet in package.DependencySets)
                {
                    foreach (var dependency in dependencySet.Dependencies)
                    {
                        //ReportDependencyPackage(targetDependency.dependencies, dependency.VersionSpec, true.Equals, packageCollection);
                        //dependentPackages.Add(repository.FindPackage(dependency.Id, dependency.VersionSpec, true, true));
                    }
                }

                //foreach (var childDependency in projectLibrary.Dependencies)
                //{
                //    var childLibrary = lockFileTargetFramework.Libraries.FirstOrDefault(library => library.Name == childDependency.Id);
                //    bool SystemReferenced = MetaPackagesSkipper.MetaPackages.Any(x => x == childLibrary.Name) || packageCollection.Any(x => x == childLibrary.Name);
                //    ReportDependency(targetDependency.dependencies, childLibrary, lockFileTargetFramework, SystemReferenced, packageCollection);
                //}
            }
        }

        private static void ReportDependency(List<Dependency> dependencies, TargetFrameworkInformation projectLibrary, bool AutoReferenced, List<string> packageCollection, LockFileTarget lockFileTargetFramework)
        {
            Dependency targetDependency = new Dependency();
            dependencies.Add(targetDependency);

            targetDependency.name = projectLibrary.FrameworkName.ToString();
            targetDependency.key = $"netframework:{projectLibrary.FrameworkName.ToString()}";
            //targetDependency.versions.Add(projectLibrary..ToNormalizedString());

            //targetDependency.checksum = "";
            //targetDependency.homepageUrl = "";
            //targetDependency.repoUrl = "";
            //targetDependency.description = projectLibrary.Version.ToFullString();
            //targetDependency.licences.Add(new licence() { name = projectLibrary.Version.ToFullString(), url = "" });

            if (!AutoReferenced)
            {
                packageCollection.Add(projectLibrary.FrameworkName.ToString());

                foreach (var childDependency in projectLibrary.Dependencies)
                {
                    var childLibrary = lockFileTargetFramework.Libraries.FirstOrDefault(library => library.Name == childDependency.Name);
                    bool SystemReferenced = MetaPackagesSkipper.MetaPackages.Any(x => x == childLibrary.Name) || packageCollection.Any(x => x == childLibrary.Name);
                    //ReportDependency(targetDependency.dependencies, childLibrary, lockFileTargetFramework, SystemReferenced, packageCollection);
                }
            }
        }
    }
}
