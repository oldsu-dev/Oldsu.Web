using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Oldsu.DatabaseServices;
using Oldsu.Logging;
using Oldsu.Types;
using Oldsu.Utils.Paginator;
using Oldsu.Web.Authentication;

namespace Oldsu.Web.Pages
{
    public class BeatmapListing : BaseLayout
    {
        public readonly IBeatmapService BeatmapService;

        private LoggingManager _loggingManager;
        public List<Beatmapset>? Listing;
        public IPaginator<Beatmapset> Paginator;
        public int TotalRows;

        public BeatmapListing(AuthenticationService authenticationService, LoggingManager loggingManager,
            IBeatmapService beatmapService)
            : base(authenticationService)
        {
            BeatmapService = beatmapService;
            _loggingManager = loggingManager;
        }

        public Beatmapset[]? BeatmapSets { get; private set; }

        [FromQuery(Name = "query")] public string? SearchQuery { get; set; } = string.Empty;
        [FromQuery(Name = "page")] public string? PageQuery { get; set; } = string.Empty;

        public async Task<IActionResult> OnGet()
        {
            if (PageQuery == string.Empty) PageQuery = "1";
            await using Database database = new();
            Paginator = BeatmapService.GetBeatmapPaginator(20);
            TotalRows = database.Beatmapsets.Count();
            var query = database.Beatmapsets.Where(b => b.OriginalBeatmapsetID == null);


            if (!string.IsNullOrEmpty(SearchQuery))
            {
                // TODO: SOMEONE OPTIMIZE THIS
                Listing = await database.Beatmapsets.Where(s => s.Title.Contains(SearchQuery) || s.Artist.Contains(SearchQuery)).ToListAsync();
                // BeatmapSets = await query.Where(s => s.Title.Contains(SearchQuery) || s.Artist.Contains(SearchQuery)).ToArrayAsync();
            }
            else
            {
                if (PageQuery != null) Listing = await Paginator.GetNewestPageAsync(short.Parse(PageQuery) - 1);
                else Listing = await Paginator.GetNewestPageAsync(0);
            }
                


            return Page();
        }
    }
}