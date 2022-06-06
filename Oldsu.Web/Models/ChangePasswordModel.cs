using Microsoft.AspNetCore.Mvc;

namespace Oldsu.Web.Models;

public class ChangePasswordModel
{
    [BindProperty(Name = "token")]
    public string Token { get; set; }
    
    [BindProperty(Name = "password")]
    public string Password { get; set; }
}