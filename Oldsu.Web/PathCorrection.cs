using System;
using System.Collections.Generic;

namespace Oldsu.Web
{
    public static class PathCorrection
    {
        public static string BaseUri => Environment.GetEnvironmentVariable("OLDSU_WEB_BASE_PATH") ?? "";

        private static readonly Dictionary<string, string> CorrectedPathCache = new Dictionary<string, string>();

        public static string Correct(string path) =>
            CorrectedPathCache[path] = CorrectedPathCache.GetValueOrDefault(path, BaseUri + path);
    }
}