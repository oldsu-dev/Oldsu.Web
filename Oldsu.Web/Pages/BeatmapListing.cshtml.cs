using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Oldsu.Logging;
using Oldsu.Types;
using Oldsu.Web.Authentication;

namespace Oldsu.Web.Pages {
    public class BeatmapListing : BaseLayout {
        public Beatmapset[]? BeatmapSets { get; private set; }

        [FromQuery(Name = "query")] public string? SearchQuery { get; set; } = string.Empty;
        
        public async Task<IActionResult> OnGet() {
            await using Database database = new();

            var query = database.Beatmapsets.Where(b => b.OriginalBeatmapsetID == null);

            if(!string.IsNullOrEmpty(SearchQuery)) {
                this.BeatmapSets = await query.Where(s => s.Title.Contains(SearchQuery) || s.Artist.Contains(SearchQuery)).ToArrayAsync();
            }
            else {
                this.BeatmapSets = await query.ToArrayAsync();
            }
            
            return this.Page();
        }

        private LoggingManager _loggingManager;
        
        public BeatmapListing(AuthenticationService authenticationService, LoggingManager loggingManager) 
            : base(authenticationService)
        {
            _loggingManager = loggingManager;
        }
    }
}