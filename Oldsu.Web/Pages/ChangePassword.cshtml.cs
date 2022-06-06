using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using Oldsu.Types;
using Oldsu.Web.Authentication;
using Oldsu.Web.Models;

namespace Oldsu.Web.Pages;

public class ChangePassword : BaseLayout
{
    public string? Token { get; set; }
    public string? PasswordChangeResult { get; set; }
    
    public async Task<IActionResult> OnGet()
    {
        string token = Request.Query["token"];
        if (token == null)
            return BadRequest();

        await using var database =  new Database();

        if (!await database.PasswordChangeTokens.AnyAsync(b => b.Token == token))
            return Unauthorized();
        
        Token = token;
        
        return Page();
    }
    
    public async Task<IActionResult> OnPost([FromForm] ChangePasswordModel model)
    {
        await using var database =  new Database();
        
        if (model.Password.Length != 64)
        {
            PasswordChangeResult = "Password did not get hashed properly, are you using an old website?";
            return Page();
        }

        try
        {
            if (await database.ChangePasswordAsync(model.Token, model.Password))
                PasswordChangeResult = "Password changed successfully!";
            else
                PasswordChangeResult = "The password was not changed.";
        }
        catch
        {
            PasswordChangeResult = "An error occurred when updating the database.";
        }

        return Page();
    }

    public ChangePassword(AuthenticationService authenticationService) : base(authenticationService)
    {
    }
}