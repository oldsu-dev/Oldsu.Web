using Microsoft.AspNetCore.Mvc;

namespace Oldsu.Web.Models
{
    public class BasicResponseModel
    {
        [BindProperty(Name = "status")]
        public string Status { get; set; }
        
        [BindProperty(Name = "message")]
        public string Message { get; set; }
    }
}