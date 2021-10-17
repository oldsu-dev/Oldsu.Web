using System;
using System.Threading.Tasks;
using HttpMultipartParser;
using Microsoft.AspNetCore.Mvc;
using Oldsu.Web.Utils;

namespace Oldsu.Web.Controllers
{
    [ApiController]
    [IgnoreAntiforgeryToken]
    [Route("/avatar")]
    public class UploadAvatar : Controller
    {
        private string _avatarFolderName = Environment.GetEnvironmentVariable("AVATAR_FILE_LOCATION");
        
        // todo check filesize
        [HttpPost]
        public async Task<IActionResult> OnPost()
        {
            await using var db = new Database();

            var hasCookie = Request.Cookies.TryGetValue("oldsu-sid", out var cookie);

            if (!hasCookie)
                return Unauthorized();

            var userSession = await SessionAuthenticator.Authenticate(cookie);

            if (userSession == null)
                return Unauthorized();

            var files = await MultipartFormDataParser.ParseAsync(HttpContext.Request.Body);

            var file = files.Files[0];

            if (file?.Name != "avatar-image")
            {
                Global.LoggingManager.LogInfo<UploadAvatar>(
                    $"{HttpContext.GetIpAddress()} sent an empty /avatar request.");
                
                return BadRequest();
            }

            await using (var stream = System.IO.File.Create($"{_avatarFolderName}\\{userSession.UserID}.png"))
                await file.Data.CopyToAsync(stream);

            return Ok();
        }
    }
}