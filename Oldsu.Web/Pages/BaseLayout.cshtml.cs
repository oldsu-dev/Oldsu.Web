using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Oldsu.Types;
using Oldsu.Web.Utils;

namespace Oldsu.Web.Pages
{
    public class BaseLayout : PageModel
    {
        public UserInfo? AuthenticatedUserInfo { get; private set; }

        public static readonly (string PageName, string Link)[] PageMenuItems = new[]
        {
            (PageName: "Home", Link: "/"),
            (PageName: "Beatmaps", Link: "/beatmaps"),
            (PageName: "Rankings", Link: "/leaderboard"),
            (PageName: "Download", Link: "/download"),
        };

        public async Task AuthenticateUserSession()
        {
            await using var db = new Database();

            if (!Request.Cookies.TryGetValue("oldsu-sid", out var sessionId))
                return;

            AuthenticatedUserInfo = await SessionAuthenticator.Authenticate(sessionId);

            if (AuthenticatedUserInfo == null)
            {
                Response.Cookies.Delete("oldsu-sid");
                return;
            }
        }
    }
}