using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
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

        database.EmailChangeTokens.Remove(token);
        await database.SaveChangesAsync();

        UserInfo userInfo = await database.UserInfo.FindAsync(token.UserID);

        if (await database.UserInfo.AnyAsync(u => u.Email == token.Email))
        {
            ChangeEmailResult = "That email is already in use.";

            return Page();
        }
        
        await using var transaction = await database.Database.BeginTransactionAsync();

        try
        {
            userInfo.Email = token.Email;

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