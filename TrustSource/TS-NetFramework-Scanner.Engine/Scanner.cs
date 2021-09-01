using TS_NetFramework_Scanner.Common;

namespace TS_NetFramework_Scanner.Engine
{
    public class Scanner
    {
        public static bool Initiate(string projectPath, string trustSourceApiKey, string trustSourceApiUrl = "", string tsBranch = "", string tsTag = "") 
        {
            VSScanner.LocateMSBuild();
            
            var vs = VSScanner.Execute(projectPath, tsBranch, tsTag);
            var ng = NuGetScanner.Execute(projectPath, tsBranch, tsTag);

            foreach(var target in vs)
            {
                var index = ng.FindIndex(t => t.moduleId.Equals(target.moduleId));
                if(index >= 0)
                {
                    target.dependencies.AddRange(ng[index].dependencies);
                    ng.RemoveAt(index);
                }
            }

            vs.AddRange(ng);

            foreach(var target in vs)
            {
                // Convert Target into Json string
                var targetJson = TargetSerializer.ConvertToJson(target);
                // Finally Post Json to Trust Source server
                TrustSourceProvider.PostScan(targetJson, trustSourceApiKey, trustSourceApiUrl);
            }

            return true;
        }
    }
}
