using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Oldsu.Web.Authentication;
using Oldsu.Web.Models;
using Oldsu.Web.Utils;

namespace Oldsu.Web.Controllers
{
    [ApiController]
    [Route("/update_information")]
    public class UpdateInformation : Controller
    {
        private AuthenticationService _authenticationService;
        
        public UpdateInformation(AuthenticationService authenticationService)
        {
            _authenticationService = authenticationService;
        } 
        
        [HttpPut]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> OnPut([FromForm] InformationUpdateModel updatedInformation)
        {
            if (_authenticationService.AuthenticatedUserInfo == null)
                return Unauthorized();

            await using var database = new Database(); 
            
            await database.UpdateInformation(_authenticationService.AuthenticatedUserInfo.UserID, 
                updatedInformation.Occupation, updatedInformation.Interests,
                updatedInformation.Birthday, updatedInformation.Discord, updatedInformation.Twitter,
                updatedInformation.Website);

            return Ok();
        }
    }
}