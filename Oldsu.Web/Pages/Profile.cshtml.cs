using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Oldsu.Web.Authentication;

namespace Oldsu.Web.Pages
{
    public class Profile : BaseLayout
    {
        public async Task<IActionResult> OnGet()
        {
            if (AuthenticatedUserInfo != null)
                return Redirect($"/u/{AuthenticatedUserInfo.UserID}");

            return Redirect("/");
        }

        public Profile(AuthenticationService authenticationService) : base(authenticationService)
        {
        }
    }
}