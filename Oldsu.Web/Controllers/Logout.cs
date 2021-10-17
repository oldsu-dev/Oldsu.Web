using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Oldsu.Web.Models;

namespace Oldsu.Web.Controllers
{
    
    // REFACTOR TO CRUD TODO
    
    [ApiController]
    [Route("/logout")]
    public class Logout : Controller
    {
        [HttpPost]
        public async Task<IActionResult> Post()
        {
            if (!Request.Cookies.TryGetValue("oldsu-sid", out var sessionId))
            {
                return Ok(new BasicResponseModel
                {
                    Status = "error",
                    Message = "You are not authenticated.",
                });
            }

            await using var db = new Database();

            await db.RemoveWebSession(sessionId);
            
            Response.Cookies.Delete("oldsu-sid");

            return Ok(new BasicResponseModel
            {
                Status = "ok",
                Message = "Logout successful."
            });
        }
    }
}