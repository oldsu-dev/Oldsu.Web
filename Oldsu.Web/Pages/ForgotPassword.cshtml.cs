using System.Linq;
using System.Threading.Tasks;
using System.Web;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using Oldsu.Types;
using Oldsu.Utils;
using Oldsu.Web.Authentication;
using Oldsu.Web.Models;
using Oldsu.Web.Utils;
using Org.BouncyCastle.Utilities.Collections;

namespace Oldsu.Web.Pages;

public class ForgotPassword : BaseLayout
{
    public bool Submitted { get; set; } = false;

    private async Task SendResetLink(string email)
    {
        try
        {
            await using var database = new Database();

            UserInfo? userInfo = await database.UserInfo.Where(u => u.Email == email).FirstOrDefaultAsync();
            
            if (userInfo == null)
                return;

            string token = TokenGenerator.GenerateToken(128);
            await database.RequirePasswordChange(userInfo.UserID, token);
            
            await EmailSender.SendAsync(userInfo.Email, "Oldsu - Change password",
                $"Here the link you can follow to change your password: https://oldsu.ayyeve.dev/change_password?token={HttpUtility.UrlEncode(token)}\n" +
                $"If you did not expect this email, CONTACT THE STAFF IMMEDIATELY.");
        }
        catch
        {
            // ignore
        }
    }
    
    public async Task<IActionResult> OnPost([FromForm] ForgetPasswordModel model)
    {
        await Task.WhenAll(Task.Delay(2000), SendResetLink(model.Email));
        Submitted = true;

        return Page();
    }

    public ForgotPassword(AuthenticationService authenticationService) : base(authenticationService)
    {
    }
}
