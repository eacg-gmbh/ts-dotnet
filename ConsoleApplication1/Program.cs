using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using NuGet;
using RestSharp;

namespace ConsoleApplication1
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            //Application.EnableVisualStyles();
            //Application.SetCompatibleTextRenderingDefault(false);
            //Application.Run(new Form1());
            var repo = new LocalPackageRepository(@"\\Mac\Home\Documents\Visual Studio 2015\Projects\ConsoleApplication1\packages");
            IQueryable<IPackage> packages = repo.GetPackages();
            ECSDependecy basePkg = new ECSDependecy
            {
                versions = new List<String> { "1.0-SNAPSHOT" },
                key = "nuget:demo",
                name = "Basismodul",
                dependencies = new List<ECSDependecy>()
            };

            OutputGraph(repo, packages, 0, basePkg);

            OutputECSGraph(0, basePkg);
            ECSArtifact artifact = new ECSArtifact
            {
                project = "Mein Demo Projekt",
                moduleId = "nuget.demo",
                module = "Artefakt 1",
                dependencies = new List<ECSDependecy> { basePkg }
            };

            var client = new RestClient("https://ecs-app.eacg.de");
            client.Proxy = new System.Net.WebProxy("http://localhost:8888");
            var request = new RestRequest("/api/v1/scans", Method.POST);
            request.RequestFormat = DataFormat.Json;
            request.AddBody(artifact);
            request.AddHeader("user-agent", "nuget-client/superc#");
            request.AddHeader("x-user", "ecsdemo@eacg.de");
            request.AddHeader("x-apikey", "a301eb79-2fd5-4ef8-820d-33df5f9a188a");
            client.Execute(request);

            //Console.ReadLine();

        }

        static void OutputGraph(LocalPackageRepository repository, IEnumerable<IPackage> packages, int depth, ECSDependecy basePkg)
        {
            foreach (IPackage package in packages)
            {
                ECSDependecy dep = new ECSDependecy
                {
                    versions = new List<string> { package.Version.ToString() },
                    key = "nuget:" + package.Id,
                    name = package.Id, //package.Title == null ? package.Title : package.Id,
                    description = package.Description,
                    homepageUrl = package.ProjectUrl?.AbsoluteUri,
                    dependencies = new List<ECSDependecy>(),
                    licenses = new List<License> { },
                };
                dep.licenses.Add(new License { name = "unknown", url = package.LicenseUrl?.AbsoluteUri });

                Console.WriteLine("{0}{1} v{2}", new string(' ', depth), package.Id, package.Version);
                basePkg.dependencies.Add(dep);

                IList<IPackage> dependentPackages = new List<IPackage>();
                foreach (var dependencySet in package.DependencySets)
                {
                    foreach (var dependency in dependencySet.Dependencies)
                    {
                        dependentPackages.Add(repository.FindPackage(dependency.Id, dependency.VersionSpec, true, true));
                    }
                }

                OutputGraph(repository, dependentPackages, depth + 3, dep);
            }
        }

        static void OutputECSGraph(int depth, ECSDependecy basePkg)
        {
            Console.WriteLine("ECS: {0}{1}:{2} v{3}", new string(' ', depth), basePkg.name, basePkg.key, basePkg.versions);
            foreach (ECSDependecy package in basePkg.dependencies)
            {
                OutputECSGraph(depth + 3, package);
            }
        }

    }

}
