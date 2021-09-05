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
            (PageName: "Rankings", Link: "/rankings"),
            (PageName: "Download", Link: "/download"),
        };

        public async Task AuthenticateUserSession()
        {
            AuthenticatedUserInfo = null;
        }
    }
}