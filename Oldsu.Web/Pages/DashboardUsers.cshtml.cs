using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using Oldsu.DatabaseServices.MySql;
using Oldsu.Enums;
using Oldsu.Types;
using Oldsu.Web.Authentication;
using Oldsu.Web.Models;

namespace Oldsu.Web.Pages;

public class DashboardUsers : PageModel
{
    public UserInfo? AuthenticatedUserInfo { get; private set; }
    public string? BanResult { get; set; }
    public UserInfo? AdminInformation { get; set; }
    public MySqlUserService UserService { get; set; }
    public DashboardUsers(AuthenticationService authenticationService, MySqlUserService userService)
    {
        AuthenticatedUserInfo = authenticationService.AuthenticatedUserInfo;
        UserService = userService;
    }
    
    public async Task<IActionResult> OnGet()
    {
        if (AuthenticatedUserInfo != null 
            && (AuthenticatedUserInfo.Privileges == Privileges.Developer || AuthenticatedUserInfo.Privileges == Privileges.BAT))
        {
            await using var db = new Database();
            AdminInformation = await db.UserInfo.FindAsync(AuthenticatedUserInfo.UserID) ?? new UserInfo();
            return Page();
        }

        return Forbid();
    }
    
    public async Task OnPost([FromForm] BanSubmitModel banData)
    {
        if (banData.Username != null) await UserService.SetUserBanByName(banData.Username, true);
        // log the action
    }
}