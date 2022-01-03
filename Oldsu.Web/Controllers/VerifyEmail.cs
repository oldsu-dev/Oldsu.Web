using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Oldsu.Web.Models;

namespace Oldsu.Web.Controllers
{
    
    [ApiController]
    [Route("/verify_email")]
    public class VerifyEmail : Controller
    {
        [HttpGet]
        public async Task<IActionResult> Get([FromQuery] string token)
        {
            await using var database = new Database();

            if (await database.CompleteEmailConfirmation(token))
                return Ok("You're email has been verified successfully. You can now login.");

            return BadRequest("Your email was not verified.");
        }
    }
}