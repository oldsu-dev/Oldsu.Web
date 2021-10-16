using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Oldsu.Types;

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

        public string ProfileLink;

        public async Task AuthenticateUserSession()
        {
            await using var db = new Database();

            if (!Request.Cookies.TryGetValue("oldsu-sid", out var sessionId))
                return;

            var session = await db.GetWebSession(sessionId);

            if (session == null)
            {
                Response.Cookies.Delete("oldsu-sid");
                return;
            }

            AuthenticatedUserInfo = session.UserInfo;
            
            ProfileLink = $"/u/{AuthenticatedUserInfo.UserID}";
        }
    }
}