using NuGet;
using NuGet.ProjectModel;
using System.Collections.Generic;
using System.Linq;
using TS_NetFramework_Scanner.Common;

namespace TS_NetFramework_Scanner.Engine.NetScanner
{
    internal class NetFrameWorkScannerExecutor
    {
        internal static Target ProcessDependencies(string solutionName, PackageSpec project, string projectPath)
        {
            HashSet<string> packageCollection = new HashSet<string>();

            Target projectTarget = new Target();
            projectTarget.project = solutionName;
            projectTarget.moduleId = $"vs:{project.Name}";
            projectTarget.module = project.Name;
            projectTarget.release = project.Version.ToFullString();

            projectTarget.dependencies = new List<Dependency>();

            foreach (var targetFramework in project.TargetFrameworks)
            {
                Dependency targetDependency = new Dependency();
                projectTarget.dependencies.Add(targetDependency);

                var frameworkName = targetFramework.FrameworkName.ToString();
                packageCollection.Add(frameworkName);

                targetDependency.name = frameworkName;
                targetDependency.key = $"netframework:{frameworkName}";
            }

            var directoryInfo = VisualStudioProvider.TryGetPackagesDirectoryInfo(project.BaseDirectory);

            if (directoryInfo == null)
            {
                directoryInfo = VisualStudioProvider.TryGetPackagesDirectoryInfo(projectPath);
            }

            var packageRepository = new NuGet.LocalPackageRepository($@"{directoryInfo.FullName}\packages");
            IQueryable<IPackage> packages = packageRepository.GetPackages();

            foreach (IPackage package in packages)
            {
                ReportDependencyPackage(packageRepository, projectTarget.dependencies, package, packageCollection);
            }

            return projectTarget;
        }

        private static void ReportDependencyPackage(LocalPackageRepository repository, List<Dependency> dependencies, IPackage package, HashSet<string> packageCollection)
        {
            Dependency targetDependency = new Dependency();
            dependencies.Add(targetDependency);

            targetDependency.key = "nuget:" + package.Id;
            targetDependency.name = package.Id;
            targetDependency.versions.Add(package.Version.ToString());

            //versions = new List<string> { package.Version.ToString() },

            //targetDependency.description = package.Description,
            //homepageUrl = package.ProjectUrl?.AbsoluteUri,

            //targetDependency.name = projectLibrary.FrameworkName.ToString();
            //targetDependency.key = $"netframework:{projectLibrary.FrameworkName.ToString()}";


            // We let add duplicate package in our Dependency tree, but won't allow its further children.
            if (packageCollection.Contains(package.GetFullName()))
            {
                return;
            }

            packageCollection.Add(package.GetFullName());

            foreach (var dependencySet in package.DependencySets)
            {
                foreach (var dependency in dependencySet.Dependencies)
                {
                    IPackage dependentPackage = repository.FindPackage(dependency.Id, dependency.VersionSpec, true, true);

                    if (dependentPackage != null)
                        ReportDependencyPackage(repository, targetDependency.dependencies, dependentPackage, packageCollection);
                }
            }
        }
    }
}
