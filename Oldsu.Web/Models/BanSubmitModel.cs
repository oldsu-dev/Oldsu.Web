using Microsoft.AspNetCore.Mvc;

namespace Oldsu.Web.Models
{
    public class BanSubmitModel
    {
        [BindProperty(Name = "username")]
        public string? Username { get; set; }
    }
}