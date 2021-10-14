using System;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Oldsu.Web.Models;
using Oldsu.Web.Utils;

namespace Oldsu.Web.Controllers
{
    [ApiController]
    [IgnoreAntiforgeryToken]
    [Route("/login")]
    public class Login : Controller
    {
        [HttpPost]
        public async Task<IActionResult> Post([FromForm] LoginSubmitModel loginData)
        {
            if (loginData.Password == null || loginData.Username == null)
            {
                return Ok(new BasicResponseModel
                {
                    Status = "error",
                    Message = "Please enter all of the fields",
                });
            }

            if (loginData.Password.Length != 64)
            {
                return Ok(new BasicResponseModel
                {
                    Status = "error",
                    Message = "Password hashing failed, are you using an obscure browser?",
                });
            }

            await using var db = new Database();

            var user = await db.AuthenticateAsync(loginData.Username, loginData.Password);

            if (user == null)
                return Ok(new BasicResponseModel
                {
                    Status = "error",
                    Message = "Unknown username or wrong password.",
                });

            var sessionId = SessionIdProvider.GetSessionId(128);

            await db.AddWebSession(sessionId, user.UserID);
            
            Response.Cookies.Append("oldsu-sid", sessionId, new CookieOptions
            {
                Expires = DateTimeOffset.Now.AddMonths(1),
                HttpOnly = true,
                Secure = true,
            });

            return Ok(new BasicResponseModel
            {
                Status = "ok",
                Message = "Login successful."
            });
        }
    }
}