using Microsoft.AspNetCore.Mvc;

namespace Oldsu.Web.Models
{
    public class RegisterSubmitModel
    {
        [BindProperty]
        public string Username { get; set; }
 
        [BindProperty]
        public string Email { get; set; }
        
        [BindProperty]
        public string Password { get; set; }
    }
}