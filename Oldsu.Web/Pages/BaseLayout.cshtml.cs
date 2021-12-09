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
            (PageName: "Home", Link:  PathCorrection.Correct("/")),
            (PageName: "Beatmaps", Link: PathCorrection.Correct("/beatmaps")),
            (PageName: "Rankings", Link: PathCorrection.Correct("/leaderboard")),
            (PageName: "Download", Link: PathCorrection.Correct("/download")),
        };
    }
}