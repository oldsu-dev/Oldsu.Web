using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using Oldsu.Web.Authentication;
using Oldsu.Web.Models;
using Oldsu.Web.Utils;

namespace Oldsu.Web.Controllers
{
    [ApiController]
    [Route("/update_bbcode")]
    public class UpdateBBCode : Controller
    {
        public AuthenticationService _authenticationService;
        
        public UpdateBBCode(AuthenticationService authenticationService)
        {
            _authenticationService = authenticationService;
        }
        
        [HttpPut]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> OnPut([FromForm] BBCodeUpdateModel updatedBBCode)
        {
            if (_authenticationService.AuthenticatedUserInfo == null)
                return Unauthorized();

            await using var db = new Database(); 
            await db.UpdateBBCode(_authenticationService.AuthenticatedUserInfo.UserID, updatedBBCode.BBCode);
            
            return Ok();
        }
    }
}