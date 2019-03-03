using System.IO;
using System.Linq;

namespace TS_NetFramework_Scanner.Engine
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

        public static DirectoryInfo TryGetPackagesDirectoryInfo(string currentPath)
        {
            var directory = new DirectoryInfo(currentPath);

            if (!directory.Exists)
                directory = directory.Parent;

            while (directory != null && !directory.GetDirectories("packages").Any())
                directory = directory.Parent;

            return directory;
        }
    }
}
