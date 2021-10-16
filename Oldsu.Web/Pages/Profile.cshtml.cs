using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Oldsu.Web.Pages
{
    public class Profile : BaseLayout
    {
        public async Task<IActionResult> OnGet()
        {
            await AuthenticateUserSession();

            if (AuthenticatedUserInfo != null)
                return Redirect($"/u/{AuthenticatedUserInfo.UserID}");

            return Redirect("/");
        }
    }
}