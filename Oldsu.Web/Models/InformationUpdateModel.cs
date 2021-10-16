using System;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc;

namespace Oldsu.Web.Models
{
    public class InformationUpdateModel
    {
        [BindProperty(Name = "occupation")]
        [StringLength(100)]
        public string? Occupation { get; set; }
        
        [BindProperty(Name = "interests")]
        [StringLength(100)]
        public string? Interests { get; set; }
        
        [BindProperty(Name = "age")]
        public DateTime? Birthday { get; set; }
        
        [BindProperty(Name = "discord")]
        [StringLength(100)]
        public string? Discord { get; set; }
        
        [BindProperty(Name = "twitter")]
        [StringLength(100)]
        public string? Twitter { get; set; }
        
        [BindProperty(Name = "website")]
        [StringLength(100)]
        public string? Website { get; set; }
    }
}