using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Oldsu.Types;

namespace Oldsu.Web.Pages {
    public class BeatmapListing : BaseLayout {
        public Beatmapset[]? BeatmapSets { get; private set; }

        [FromQuery(Name = "query")] public string? SearchQuery { get; set; } = string.Empty;
        
        public async Task<IActionResult> OnGet() {
            await using Database database = new();

            if(!string.IsNullOrEmpty(SearchQuery)) {
                this.BeatmapSets = await database.Beatmapsets.Where(s => s.Title.Contains(SearchQuery) || s.Artist.Contains(SearchQuery)).ToArrayAsync();
            }
            else {
                this.BeatmapSets = await database.Beatmapsets.ToArrayAsync();
            }

            
            return this.Page();
        }
    }
}