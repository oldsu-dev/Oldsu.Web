using Microsoft.AspNetCore.Mvc;

namespace Oldsu.Web.Models;

public class ForgetPasswordModel
{
    [BindProperty(Name = "email")]
    public string Email { get; set; }
}