using Microsoft.AspNetCore.Mvc;

namespace Oldsu.Web.Models;

public class ChangeEmailModel
{
    [BindProperty(Name = "email")]
    public string Email { get; set; }
}