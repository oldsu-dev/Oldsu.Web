using Microsoft.AspNetCore.Mvc;

namespace Oldsu.Web.Models
{
    public class LoginSubmitModel
    {
        [BindProperty(Name = "username")]
        public string? Username { get; set; }

        [BindProperty(Name = "password")]
        public string? Password { get; set; }
    }
}