using System;
using System.Net;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Primitives;
using Oldsu.Enums;
using Oldsu.Logging;
using Oldsu.Logging.Strategies;
using Oldsu.Utils.Location;
using Oldsu.Web.Authentication;
using Oldsu.Web.Models;
using Oldsu.Web.Utils;

namespace Oldsu.Web.Pages
{
    public class Register : BaseLayout
    {
        public string? RegistrationResult { get; set; }
        
        public async Task OnPost([FromForm] RegisterSubmitModel registerData)
        {
            await using var database = new Database();
            
            var hasCookie = Request.Cookies.TryGetValue("oldsu-sid", out var cookie);

            if (hasCookie)
            {
                var userSession = await database.GetWebSession(cookie);

                if (userSession != null)
                {
                    await _loggingManager.LogCritical<Register>(
                        $"{HttpContext.GetIpAddress()}, {userSession.UserID} attempted to create an multiaccount.");
                    
                    RegistrationResult = "Staff have been notified of your attempt to create a multiaccount.";

                    return;
                }
            }

            if (registerData.Email == null || registerData.Password == null || registerData.Username == null)
            {
                RegistrationResult = "Some values were not entered, please assign values to every fields.";
                return;
            }

            if (registerData.Password.Length != 64)
            {
                RegistrationResult = "Password did not get hashed properly, are you using an old website?";
                return;
            }

            if (!HttpContext.Request.Headers.TryGetValue("CF-Connecting-IP", out var ip))
                ip = "127.0.0.1"; 
            
            var attemptResult = await database.ValidateRegistrationAttempt(registerData.Username, ip);
            
            switch (attemptResult)
            {
                case RegisterAttemptResult.IpAlreadyRegistered:
                    await _loggingManager.LogCritical<Register>(
                        $"Username: {registerData.Username} has an already registered ip: {HttpContext.GetIpAddress()}.");
                    goto case RegisterAttemptResult.RegisterSuccessful;
                    
                case RegisterAttemptResult.RegisterSuccessful:
                    var (_, _, country) = await Geolocation.GetGeolocationAsync(ip);
                    
                    await database.RegisterAsync(
                        registerData.Username, 
                        registerData.Email,
                        registerData.Password, 
                        country);

                    RegistrationResult = "Registration was successful! Please login to continue.";
                    break;

                case RegisterAttemptResult.UsernameAlreadyExists:
                    RegistrationResult = "Username already exists, please use another one.";
                    break;
            }
        }

        private LoggingManager _loggingManager;
        
        public Register(LoggingManager loggingManager, AuthenticationService authenticationService) : base(authenticationService)
        {
            _loggingManager = loggingManager;
        }
    }
}