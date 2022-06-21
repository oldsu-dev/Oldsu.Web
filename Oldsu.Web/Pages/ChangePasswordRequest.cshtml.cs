using System.Threading.Tasks;
using System.Web;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Oldsu.Utils;
using Oldsu.Web.Authentication;
using Oldsu.Web.Utils;

namespace Oldsu.Web.Pages;

public class ChangePasswordRequest : BaseLayout
{
    public ChangePasswordRequest(AuthenticationService authenticationService) : base(authenticationService)
    {
    }
    
    public async Task<IActionResult> OnPost()
    {
        if (AuthenticatedUserInfo == null)
            return Unauthorized();

        string token = TokenGenerator.GenerateToken(128);

        await using Database database = new Database();
        await database.RequirePasswordChange(AuthenticatedUserInfo.UserID, token);

        await EmailSender.SendAsync(AuthenticatedUserInfo.Email, "Oldsu - Change password",
            $"Here the link you can follow to change your password: https://oldsu.ayyeve.xyz/change_password?token={HttpUtility.UrlEncode(token)}\n" +
            $"If you did not expect this email, CONTACT THE STAFF IMMEDIATELY.");

        return Page();
    }

    public IActionResult OnGet()
    {
        return Unauthorized();
    }

}