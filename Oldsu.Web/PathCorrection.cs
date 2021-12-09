using System;

namespace Oldsu.Web
{
    public static class PathCorrection
    {
        public static string BaseUri => Environment.GetEnvironmentVariable("OLDSU_WEB_BASE_PATH") ?? "";

        public static string Correct(string path) => BaseUri + path;
    }
}