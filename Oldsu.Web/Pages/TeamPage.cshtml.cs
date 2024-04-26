using Microsoft.AspNetCore.Mvc.RazorPages;
using Oldsu.Web.Authentication;

namespace Oldsu.Web.Pages
{
    public class TeamPage : BaseLayout
    {
        public void OnGet()
        {
            
        }

        public TeamPage(AuthenticationService authenticationService) : base(authenticationService)
        {
        }
    }
}