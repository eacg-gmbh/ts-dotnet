using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace TS_NetCore_Scanner.Engine
{

    public static class VisualStudioProvider
    {
        public static string GetSolutionName(string currentPath)
        {
            DirectoryInfo directory = TryGetSolutionDirectoryInfo(currentPath);

            if (directory != null)
            {
                var solutionFile = directory.GetFiles("*.sln").FirstOrDefault();
                string fileName = Path.GetFileNameWithoutExtension(solutionFile.Name);

                return fileName;
            }

            return "";
        }


        public static DirectoryInfo TryGetSolutionDirectoryInfo(string currentPath)
        {
            var directory = new DirectoryInfo(currentPath);

            if (!directory.Exists)
                directory = directory.Parent;

            while (directory != null && !directory.GetFiles("*.sln").Any())
                directory = directory.Parent;

            return directory;
        }
    }
}
