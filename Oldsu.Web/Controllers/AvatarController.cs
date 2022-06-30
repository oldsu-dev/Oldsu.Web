using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Oldsu.Types;

namespace Oldsu.Web.Controllers;

[ApiController]
[Route("/avatars/{id:int}.png")]
public class AvatarController : Controller
{
    private readonly IWebHostEnvironment _env;

    public AvatarController(IWebHostEnvironment env) =>
        _env = env;
    
    [HttpGet]
    public async Task<IActionResult> GetController(uint id)
    {
        await using var db = new Database();

        UserInfo? userInfo = await db.UserInfo.FindAsync(id);

        if (userInfo == null || userInfo.Banned)
            return NotFound();

        var filePath = Path.Combine(
            _env.ContentRootPath, FolderConfiguration.AvatarsFolder, $"{id}.png");

        return PhysicalFile(filePath, "image/png");
    }
}