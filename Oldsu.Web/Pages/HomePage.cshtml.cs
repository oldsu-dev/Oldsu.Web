using System;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using Oldsu.Types;
using Oldsu.Web.Validators;

namespace Oldsu.Web.Pages
{
    public class HomePage : BaseLayout
    {
        public News? LatestNews { get; private set; }
    
        public async Task<IActionResult> OnGet()
        {
            await using var database = new Database();
            LatestNews = await database.News.OrderByDescending(n => n.Date).FirstOrDefaultAsync();
            
            return Page();
        }
    }
}