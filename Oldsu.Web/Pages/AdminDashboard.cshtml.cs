using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Oldsu.DatabaseServices;
using Oldsu.Enums;
using Oldsu.Logging.Strategies;
using Oldsu.Types;
using Oldsu.Utils;
using Oldsu.Web.Authentication;

namespace Oldsu.Web.Pages
{
    public class AdminDashboard : PageModel
    {
        public UserInfo? AuthenticatedUserInfo { get; private set; }
        public Types.UserInfo AdminInformation { get; set; }

        public CurrentMenu _CurrentMenu;

        public readonly IBeatmapService BeatmapService;

        public OnlineUserService OnlineUserService;

        public enum CurrentMenu
        {
            Dashboard = 0,
            ServerStatus = 1,
            Beatmaps = 2,
            Users = 3,
            Reports = 4
        }
        
        public AdminDashboard(AuthenticationService authenticationService, 
            IBeatmapService beatmapService, OnlineUserService onlineUserService)
        {
            AuthenticatedUserInfo = authenticationService.AuthenticatedUserInfo;
            BeatmapService = beatmapService;
            OnlineUserService = onlineUserService;
        }

        public async Task<IActionResult> OnGet()
        {
            if (AuthenticatedUserInfo == null 
                || AuthenticatedUserInfo.Privileges == Privileges.Normal 
                || AuthenticatedUserInfo.Privileges == Privileges.Supporter)
                return Forbid();
                
            await using var db = new Database();
            AdminInformation = await db.UserInfo.FindAsync(AuthenticatedUserInfo.UserID) ?? new Types.UserInfo();

            var users = OnlineUserService.GetOnlineUsers();
            
            foreach (OnlineUser? user in users.Result)
            {
                System.Console.WriteLine("----");
                System.Console.WriteLine(user?.Username);
                System.Console.WriteLine("----");
            }
            
            
            return Page();
        }
        
    }
}