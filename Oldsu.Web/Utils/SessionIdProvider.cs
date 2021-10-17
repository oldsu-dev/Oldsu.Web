using System;
using System.Security.Cryptography;

namespace Oldsu.Web.Utils
{
    public class SessionIdProvider
    {
        public static string GetSessionId(uint length)
        {
            using var rng = new RNGCryptoServiceProvider();
            var buffer = new byte[length/2];
            rng.GetBytes(buffer);

            return BitConverter.ToString(buffer).Replace("-", "");
        }
    }
}