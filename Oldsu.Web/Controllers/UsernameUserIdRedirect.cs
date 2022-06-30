using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Oldsu.Types;
using Oldsu.Utils.Cache;

namespace Oldsu.Web.Controllers;

[ApiController]
[Route("/u/{username:alpha}/{mode?}")]
public class UsernameUserIdRedirect : Controller
{
    [HttpGet()]
    public async Task<IActionResult> Get(string username, string? mode)
    {
        await using var database = new Database();

        int? id = await database.UserInfo.Where(u => u.Username == username)
            .Select(u => (int?)u.UserID).FirstOrDefaultAsync();

        if (id == null)
            return NotFound();
        
        return Redirect($"/u/{id}/{mode ?? string.Empty}");
    }
}