using System;
using System.IO;
using System.Threading.Tasks;
using HttpMultipartParser;
using Microsoft.AspNetCore.Mvc;
using Oldsu.Logging;
using Oldsu.Web.Authentication;
using Oldsu.Web.Utils;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.Processing;

namespace Oldsu.Web.Controllers
{
    [ApiController]
    [Route("/avatar")]
    public class UploadAvatar : Controller
    {
        private AuthenticationService _authenticationService;
        private LoggingManager _loggingManager;
        
        public UploadAvatar(AuthenticationService authenticationService, LoggingManager loggingManager)
        {
            _authenticationService = authenticationService;
        }
        
        // todo check filesize
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> OnPost()
        {
            if (_authenticationService.AuthenticatedUserInfo == null)
                return Unauthorized();
            
            var files = await MultipartFormDataParser.ParseAsync(HttpContext.Request.Body);
            var file = files.Files[0];

            if (file?.Name != "avatar-image")
            {
                await _loggingManager.LogInfo<UploadAvatar>(
                    $"{HttpContext.GetIpAddress()} sent an empty /avatar request.");
                
                return BadRequest();
            }

            using (var image = await Image.LoadAsync(file.Data))
            {
                int width = image.Width, height = image.Height;
                
                image.Mutate(x =>
                {
                    int newX = 0, newY = 0, newWidth = width, newHeight = height;

                    if (height < width)
                    {
                        newX = (width - height) / 2;
                        newY = 0;
                        newWidth = newHeight = height;
                    } 
                    else if (height > width)
                    {
                        newX = 0;
                        newY = (height - width) / 2;
                        newWidth = newHeight = width;
                    }

                    x.Crop(new Rectangle(newX, newY, newWidth, newHeight)).Resize(128, 128);
                });

                await image.SaveAsPngAsync(
                    $"{FolderConfiguration.AvatarsFolder}/{_authenticationService.AuthenticatedUserInfo.UserID}.png");
            }

            await using var database = new Database();

            (await database.UserInfo.FindAsync(_authenticationService.AuthenticatedUserInfo.UserID)).HasAvatar = true;

            await database.SaveChangesAsync();

            return Ok();
        }
    }
}