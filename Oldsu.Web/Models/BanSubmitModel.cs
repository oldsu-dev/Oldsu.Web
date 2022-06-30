using Microsoft.AspNetCore.Mvc;
using Oldsu.Types;

namespace Oldsu.Web.Models
{
    public class BanSubmitModel
    {
        [BindProperty(Name = "username")]
        public string? Username { get; set; }
        
        [BindProperty(Name = "reason")]
        public string? Reason { get; set; }

        [BindProperty(Name = "admin")] 
        public string Admin { get; set; }
    }
}