using System.IO;

namespace HRMS.Utilities
{
    public static class PathHelper
    {
        public static string Combine(string path1, string path2)
        {
            var path = Path.Combine(path1, path2);
            return NormalizeCrossPlatformPath(path);
        }

        public static string Combine(string path1, string path2, string path3)
        { 
            var path = Path.Combine(path1, path2, path3);
            return NormalizeCrossPlatformPath(path);
        }

        public static string Combine(string path1, string path2, string path3, string path4)
        {
            var path = Path.Combine(path1, path2, path3, path4);
            return NormalizeCrossPlatformPath(path);
        }

        public static string Combine(params string[] paths)
        {
            var path = Path.Combine(paths);
            return NormalizeCrossPlatformPath(path);
        }

        public static string NormalizeCrossPlatformPath(string path)
        {
            return string.IsNullOrEmpty(path) ? path : path.Replace(Path.DirectorySeparatorChar, Path.AltDirectorySeparatorChar);
        }

        public static string GetDirectoryName(string path)
        {
            var dirPath = Path.GetDirectoryName(path);

            return NormalizeCrossPlatformPath(dirPath);
        }
        public static string GetCurrentDirectory()
        {
            var dirPath = Directory.GetCurrentDirectory();

            return NormalizeCrossPlatformPath(dirPath);
        }

        public static void DeleteDirectory(string targetDir)
        {
            string[] files = Directory.GetFiles(targetDir);
            string[] dirs = Directory.GetDirectories(targetDir);

            foreach (string file in files)
            {
                File.SetAttributes(file, FileAttributes.Normal);
                File.Delete(file);
            }

            foreach (string dir in dirs)
            {
                DeleteDirectory(dir);
            }

            Directory.Delete(targetDir, false);
        }
    }
}