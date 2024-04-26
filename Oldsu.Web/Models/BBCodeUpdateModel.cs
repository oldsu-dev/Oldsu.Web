using Microsoft.AspNetCore.Mvc;

namespace Oldsu.Web.Models
{
    public class BBCodeUpdateModel
    {
        [BindProperty(Name = "bbcode")] 
        public string? BBCode { get; set; }
    }
}