using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using Oldsu.Types;

namespace Oldsu.Web.Pages {
    public class BeatmapListing : BaseLayout {
        public Beatmapset[]? BeatmapSets { get; private set; }
        
        public async Task<IActionResult> OnGet() {
            await using Database database = new();

            this.BeatmapSets = await database.Beatmapsets.ToArrayAsync();
            
            return this.Page();
        }
    }
}