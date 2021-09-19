using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Oldsu.Enums;
using Oldsu.Logging;
using Oldsu.Logging.Strategies;
using Oldsu.Utils.Location;
using Oldsu.Web.Models;

namespace Oldsu.Web.Pages
{
    [IgnoreAntiforgeryToken]
    public class Register : BaseLayout
    {
        public string? RegistrationResult { get; set; }

        public void OnGet()
        {
            
        }

        public async Task OnPost(RegisterSubmitModel registerData)
        {
            await using var database = new Database();

            if (registerData.Password.Length != 64)
            {
                RegistrationResult = "Password did not get hashed properly, are you using an old website?";
                return;
            }
            
            if (registerData.Email == null || registerData.Password == null || registerData.Username == null)
            {
                RegistrationResult = "Some values were not entered, please assign values to every fields.";
                return;
            }

            var registerResultTask = database.ValidateUser(
                registerData.Username, HttpContext.GetServerVariable("HTTP_X_FORWARDED_FOR"));
            
            var geolocTask = Geolocation.GetGeolocationAsync(
                HttpContext.GetServerVariable("HTTP_X_FORWARDED_FOR"));
            
            switch (await registerResultTask)
            {
                case RegisterResult.IpAlreadyRegistered:
                    await Global.LoggingManager.LogCritical<Register>(
                        $"Username: {registerData.Username} has an already registered ip: {HttpContext.GetServerVariable("HTTP_X_FORWARDED_FOR")}.");
                    goto case RegisterResult.RegisterSuccessful;
                    
                case RegisterResult.RegisterSuccessful:
                    await database.RegisterAsync(
                        registerData.Username, 
                        registerData.Email,
                        registerData.Password, 
                        (await geolocTask).Country);

                    RegistrationResult = "Registration was successful!";
                    break;

                case RegisterResult.UsernameAlreadyExists:
                    RegistrationResult = "Username already exists, please use another one.";
                    break;
            }
        }
    }
}