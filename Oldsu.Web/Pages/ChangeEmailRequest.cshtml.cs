using System;
using System.Net.Mail;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Web;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Oldsu.Types;
using Oldsu.Utils;
using Oldsu.Web.Authentication;
using Oldsu.Web.Models;
using Oldsu.Web.Utils;

namespace Oldsu.Web.Pages;

public class ChangeEmailRequest : BaseLayout
{
    public string? ChangeEmailRequestError { get; set; }

    public IActionResult OnGet() => BadRequest();
    
    public async Task<IActionResult> OnPost([FromForm] ChangeEmailModel model)
    {        
        if (AuthenticatedUserInfo == null)
            return Unauthorized();

        if (!MailAddress.TryCreate(model.Email, out _))
        {
            ChangeEmailRequestError = "Invalid email provided.";
            return Page();
        }

        string token = TokenGenerator.GenerateToken(128);

        await using var database = new Database();
        await using var transaction = await database.Database.BeginTransactionAsync();

        try
        {
            database.EmailChangeTokens.Add(new EmailChangeToken
            {
                Token = token,
                Email = model.Email,
                UserID = AuthenticatedUserInfo.UserID
            });

            await database.SaveChangesAsync();
            await transaction.CommitAsync();
        }
        catch
        {
            ChangeEmailRequestError = "An error occurred when contacting the database.";
            await transaction.RollbackAsync();

            return Page();
        }

        await EmailSender.SendAsync(model.Email, "Email change request",
            $"Here the link you can follow to change your email: https://oldsu.ayyeve.xyz/dev/site/change_email?token={HttpUtility.UrlEncode(token)}\n" +
            $"If you did not expect this email, just ignore it.");

        return Page();
    }

    public ChangeEmailRequest(AuthenticationService authenticationService) : base(authenticationService)
    {
    }
}