using System;
using System.IO;

namespace Oldsu.Web
{
    /// <summary>
    ///     Class for custom configurations
    /// </summary>
    public static class FolderConfiguration
    {
        public static string AvatarsFolder { get; } =
            Environment.GetEnvironmentVariable("AVATAR_FILE_LOCATION") ?? "avatars/";
        
        public static void CreateFolders()
        {
            Directory.CreateDirectory(AvatarsFolder);
        }
    }
}