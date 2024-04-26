using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Oldsu.Web.Authentication;

namespace Oldsu.Web.Pages
{
    public class Dashboard : BaseLayout
    {
        public Types.UserPage UserPageInformation { get; set; }

        public async Task<IActionResult> OnGet()
        {
            if (AuthenticatedUserInfo == null)
                return Unauthorized();
            
            await using var db = new Database();
            UserPageInformation = await db.UserPages.FindAsync(AuthenticatedUserInfo.UserID) ?? new Types.UserPage();

            return Page();
        }

        public Dashboard(AuthenticationService authenticationService) : base(authenticationService)
        {
        }
    }
}