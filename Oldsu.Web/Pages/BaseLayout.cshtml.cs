using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Oldsu.Types;
using Oldsu.Web.Authentication;
using Oldsu.Web.Utils;

namespace Oldsu.Web.Pages
{
    public class BaseLayout : PageModel
    {
        public UserInfo? AuthenticatedUserInfo { get; private set; }
        
        public BaseLayout(AuthenticationService authenticationService)
        {
            AuthenticatedUserInfo = authenticationService.AuthenticatedUserInfo;
        } 
        
        public static readonly (string PageName, string Link)[] PageMenuItems = new[]
        {
            (PageName: "Home", Link: "/"),
            (PageName: "Beatmaps", Link: "/beatmaps"),
            (PageName: "Rankings", Link: "/leaderboard"),
            (PageName: "Download", Link: "/download"),
        };
    }
}