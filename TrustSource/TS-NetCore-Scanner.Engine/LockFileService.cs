using NuGet.ProjectModel;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TS_NetCore_Scanner.Engine
{
    internal class LockFileService
    {
        public LockFile GetLockFile(string projectPath, string outputPath)
        {
            // Run the restore command
            DotNetRunner dotNetRunner = new DotNetRunner("nuget");
            string[] arguments = new[] { "restore", $"\"{projectPath}\"" };
            _ = dotNetRunner.Run(Path.GetDirectoryName(projectPath), arguments);

            // Load the lock file
            string lockFilePath = Path.Combine(outputPath, "project.assets.json");
            return LockFileUtilities.GetLockFile(lockFilePath, NuGet.Common.NullLogger.Instance);
        }
    }
}
