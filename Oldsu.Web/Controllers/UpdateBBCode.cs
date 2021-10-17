using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using Oldsu.Web.Models;
using Oldsu.Web.Utils;

namespace Oldsu.Web.Controllers
{
    [ApiController]
    [Route("/update_bbcode")]
    public class UpdateBBCode : Controller
    {
        [HttpPut]
        public async Task<IActionResult> OnPut([FromForm] BBCodeUpdateModel updatedBBCode)
        {
            await using var db = new Database(); // todo move this to middleware

            var hasCookie = Request.Cookies.TryGetValue("oldsu-sid", out var cookie);

            if (!hasCookie)
                return Unauthorized();

            var userSession = await SessionAuthenticator.Authenticate(cookie);

            if (userSession == null)
                return Unauthorized();

            await db.UpdateBBCode(userSession.UserID, updatedBBCode.BBCode);
            
            return Ok();
        }
    }
}