using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Oldsu.Types;
using Oldsu.Utils;
using Oldsu.Web.Authentication;

namespace Oldsu.Web.Pages;

public class ChangeEmail : BaseLayout
{
    public string? ChangeEmailResult { get; set; }
    
    public async Task<IActionResult> OnGet()
    {
        string tokenIdentifier = Request.Query["token"];
        if (tokenIdentifier == null)
            return BadRequest();

        await using var database =  new Database();
 
        EmailChangeToken? token = await database.EmailChangeTokens.FindAsync(tokenIdentifier);

        if (token == null)
            return Unauthorized();
        
        UserInfo userInfo = await database.UserInfo.FindAsync(token.UserID);
        
        await using var transaction = await database.Database.BeginTransactionAsync();

        try
        {
            database.EmailChangeTokens.Remove(token);
            userInfo.Email = token.Email;

            await database.SaveChangesAsync();
            await transaction.CommitAsync();
        }
        catch
        {
            ChangeEmailResult = "An error occurred when contacting the database.";
            await transaction.RollbackAsync();

            return Page();
        }

        ChangeEmailResult = "Email changed successfully.";
        
        return Page();
    }

    public ChangeEmail(AuthenticationService authenticationService) : base(authenticationService)
    {
    }
}