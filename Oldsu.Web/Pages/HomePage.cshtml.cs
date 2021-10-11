using System;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using Oldsu.Types;
using Oldsu.Web.Models;
using Oldsu.Web.Validators;

namespace Oldsu.Web.Pages
{
    [IgnoreAntiforgeryToken]
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