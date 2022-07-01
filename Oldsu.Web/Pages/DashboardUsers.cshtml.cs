using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using Oldsu.DatabaseServices.MySql;
using Oldsu.Enums;
using Oldsu.Logging;
using Oldsu.Types;
using Oldsu.Web.Authentication;
using Oldsu.Web.Models;

namespace Oldsu.Web.Pages;

public class DashboardUsers : PageModel
{
    public UserInfo AuthenticatedUserInfo { get; private set; }
    public string BanResult { get; set; } = "";
    public UserInfo AdminInformation { get; set; }
    public MySqlUserService UserService { get; set; }
    private LoggingManager _loggingManager;
    public string BanMessage { get; set; } = "";
    
    public DashboardUsers(AuthenticationService authenticationService, MySqlUserService userService,
        LoggingManager loggingManager)
    {
        AuthenticatedUserInfo = authenticationService.AuthenticatedUserInfo;
        UserService = userService;
        _loggingManager = loggingManager;
    }
    
    public async Task<IActionResult> OnGet()
    {
        if (AuthenticatedUserInfo != null 
            && (AuthenticatedUserInfo.Privileges == Privileges.Developer))
        {
            await using var db = new Database();
            AdminInformation = await db.UserInfo.FindAsync(AuthenticatedUserInfo.UserID) ?? new UserInfo();
            return Page();
        }
        
        return Forbid();
    }
    
    public async Task OnPostBanUsername([FromForm] BanSubmitModel banData)
    {
        if (AuthenticatedUserInfo != null
            && (AuthenticatedUserInfo.Privileges == Privileges.Developer))
        {
            await using var database = new Database();
            AdminInformation = await database.UserInfo.FindAsync(AuthenticatedUserInfo.UserID) ?? new UserInfo();
            banData.Admin = AdminInformation;
            var userInfo = database.UserInfo.Where(u => u.Username == banData.Username).FirstOrDefaultAsync().Result;
            if (userInfo != null) banData.UserID = (int) userInfo.UserID;
            if (banData.Username != null && banData.Reason != null)
            {
                try
                {
                    await UserService.SetUserBanByName(banData.Username, true, banData.Reason);
                    BanMessage =
                        $"User {banData.Username} is banned by {banData.Admin.Username}.\nReason: {banData.Reason}";
                    await _loggingManager.LogInfo<DashboardUsers>(BanMessage);
                }
                catch
                {
                    BanMessage = "Can't find user with this specific username";
                }
            }
            
        }
    }
    
    public async Task OnPostBanUserID([FromForm] BanSubmitModel banData)
    {
        if (AuthenticatedUserInfo != null
            && (AuthenticatedUserInfo.Privileges == Privileges.Developer))
        {
            await using var database = new Database();
            AdminInformation = await database.UserInfo.FindAsync(AuthenticatedUserInfo.UserID) ?? new UserInfo();
            banData.Admin = AdminInformation;
            var userInfo = database.UserInfo.Where(u => u.UserID == banData.UserID).FirstOrDefaultAsync().Result;
            if (userInfo != null) banData.Username = userInfo.Username;
            if (banData.Username != null && banData.Reason != null)
            {
                try
                {
                    await UserService.SetUserBanByID(banData.UserID, true, banData.Reason);
                    BanMessage = $"User {banData.Username} is banned by {banData.Admin.Username}.\nReason: {banData.Reason}";
                    await _loggingManager.LogInfo<DashboardUsers>(BanMessage);
                }
                catch
                {
                    BanMessage = "Can't find user with that id";
                }
            }
        }
    }
}