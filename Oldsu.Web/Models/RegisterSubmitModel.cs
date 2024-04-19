using Microsoft.AspNetCore.Mvc;

namespace Oldsu.Web.Models
{
    public class RegisterSubmitModel
    {
        [BindProperty(Name = "username")]
        public string? Username { get; set; }
 
        [BindProperty(Name = "email")]
        public string? Email { get; set; }
        
        [BindProperty(Name = "password")]
        public string? Password { get; set; }
    }
}