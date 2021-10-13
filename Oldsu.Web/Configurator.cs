using System;
using System.IO;

namespace Oldsu.Web
{
    /// <summary>
    ///     Class for custom configurations
    /// </summary>
    public static class Configurator
    {
        private static string[] _folderNames = new[]
        {
            Environment.GetEnvironmentVariable("AVATAR_FILE_LOCATION")
        };
        
        public static void CreateFolders()
        {
            foreach (var folderName in _folderNames)
            {
                Directory.CreateDirectory(folderName);
            }
        }
    }
}