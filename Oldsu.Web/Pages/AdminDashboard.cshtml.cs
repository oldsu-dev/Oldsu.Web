using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Oldsu.Enums;
using Oldsu.Types;
using Oldsu.Web.Authentication;

namespace Oldsu.Web.Pages
{
    public class AdminDashboard : PageModel
    {
        public UserInfo? AuthenticatedUserInfo { get; private set; }
        public Types.UserInfo AdminInformation { get; set; }

        public CurrentMenu _CurrentMenu;

        public enum CurrentMenu
        {
            Dashboard = 0,
            ServerStatus = 1,
            Beatmaps = 2,
            Users = 3,
            Reports = 4
        }
        
        public AdminDashboard(AuthenticationService authenticationService)
        {
            AuthenticatedUserInfo = authenticationService.AuthenticatedUserInfo;
        }

        public async Task<IActionResult> OnGet()
        {
            if (AuthenticatedUserInfo == null 
                || AuthenticatedUserInfo.Privileges == Privileges.Normal 
                || AuthenticatedUserInfo.Privileges == Privileges.Supporter)
                return Forbid();
                
            await using var db = new Database();
            AdminInformation = await db.UserInfo.FindAsync(AuthenticatedUserInfo.UserID) ?? new Types.UserInfo();
            //db.TestAddMapAsync("f4b98cf7c6e2d9eed80f4551da211ac3");
            //db.TestAddMapAsync("f4b98cf7c6e2d9eed80f4551da211ac3");
            //db.TestAddMapAsync("f4b98cf7c6e2d9eed80f4551da211ac3");
            return Page();
        }
        
    }
}