using NuGet.ProjectModel;
using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.InteropServices;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TS_NetCore_Scanner.Engine
{
    public class LockFileService
    {
        public LockFile GetLockFile(string projectPath, string outputPath)
        {
            // Run the restore command
            

            DotNetRunner dotNetRunner = new DotNetRunner("nuget", !RuntimeInformation.IsOSPlatform(OSPlatform.OSX));
            string[] arguments = new[] { "restore", $"\"{projectPath}\"" };
            try
            {
                _ = dotNetRunner.Run(Path.GetDirectoryName(projectPath), arguments);
            } catch (Exception ex)
            {
                System.Console.WriteLine("An error occured while executing NuGet");
                throw ex;
            }

            // Load the lock file
            string lockFilePath = Path.Combine(outputPath, "project.assets.json");
            return LockFileUtilities.GetLockFile(lockFilePath, NuGet.Common.NullLogger.Instance);
        }
    }
}
