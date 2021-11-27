using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Oldsu.Types;
using Oldsu.Web.Utils;

namespace Oldsu.Web.Authentication
{
    public class AuthenticationMiddleware
    {
        private readonly RequestDelegate _next;

        public AuthenticationMiddleware(RequestDelegate next)
        {
            _next = next;
        }
        
        public async Task InvokeAsync(HttpContext context, AuthenticationService authenticationService)
        {
            await using var db = new Database();

            if (context.Request.Cookies.TryGetValue("oldsu-sid", out var sessionId))
            {
                UserInfo? userInfo = await SessionAuthenticator.Authenticate(sessionId!);

                if (userInfo != null)
                    authenticationService.AuthenticatedUserInfo = userInfo;
                else
                    context.Response.Cookies.Delete("oldsu-sid");
            }

            // Call the next delegate/middleware in the pipeline
            await _next(context);
        }
    }
}