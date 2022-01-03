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
                return Redirect(PathCorrection.Correct($"/u/{AuthenticatedUserInfo.UserID}"));

            return Redirect(PathCorrection.Correct("/"));
        }

        public Profile(AuthenticationService authenticationService) : base(authenticationService)
        {
        }
    }
}