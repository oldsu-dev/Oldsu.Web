using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Oldsu.Web.Models;
using Oldsu.Web.Utils;

namespace Oldsu.Web.Controllers
{
    [ApiController]
    [Route("/update_information")]
    public class UpdateInformation : Controller
    {
        [HttpPut]
        public async Task<IActionResult> OnPut([FromForm] InformationUpdateModel updatedInformation)
        {
            await using var db = new Database(); // todo move this to middleware

            var hasCookie = Request.Cookies.TryGetValue("oldsu-sid", out var cookie);

            if (!hasCookie)
                return Unauthorized();

            var userSession = await SessionAuthenticator.Authenticate(cookie);

            if (userSession == null)
                return Unauthorized();

            await db.UpdateInformation(userSession.UserID, updatedInformation.Occupation, updatedInformation.Interests,
                updatedInformation.Birthday, updatedInformation.Discord, updatedInformation.Twitter,
                updatedInformation.Website);

            return Ok();
        }
    }
}