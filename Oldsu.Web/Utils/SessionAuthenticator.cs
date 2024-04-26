using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Oldsu.Types;
using Oldsu.Utils.Cache;

namespace Oldsu.Web.Utils
{
    public class SessionAuthenticator
    {
        /// <summary>
        ///     Authenticates session using session id.
        /// </summary>
        /// <returns>Session's user info</returns>
        
        public static async Task<UserInfo?> Authenticate(string sessionId)
        {
            await using var db = new Database();

            var session = await db.GetWebSession(sessionId);

            if (session == null) return null;
            if (session.ExpiresAt >= DateTime.Now) return session.UserInfo; // if session is still not expired its OK

            await db.RemoveWebSession(sessionId);
            return null;
        }
        
        /* yagni
        public static async Task<UserInfo> Authenticate(string sessionId)
        {
            await using var db = new Database();

            var (isFoundFromCache, session) = await _sessionCache.TryGetValue(sessionId);

            if (isFoundFromCache)
            {
                if (session.ExpiresAt < DateTime.Now)
                {
                    await _sessionCache.TryRemove(sessionId);
                    await db.RemoveWebSession(sessionId);

                    return null;
                }
            }

            session = await db.GetWebSession(sessionId);

            if (session != null)
            {
                if (session.ExpiresAt < DateTime.Now)
                {
                    await db.RemoveWebSession(sessionId);

                    return null;
                }
                
                _sessionCache.TryAdd(sessionId, session, DateTime.Now.AddMinutes(1));
                return session.UserInfo;
            } 
        }
        */
    }
}