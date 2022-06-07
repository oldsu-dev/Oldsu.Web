using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Primitives;
using Oldsu.Enums;
using Oldsu.Types;
using Oldsu.Web.Authentication;

namespace Oldsu.Web.Pages
{
    public class Leaderboard : BaseLayout
    {
        private const int PerPageRanks = 50;
        
        public StatsWithRank[] Stats { get; private set; }

        [FromQuery(Name = "query")] 
        public string? SearchQuery { get; set; } = string.Empty;

        [FromQuery(Name = "mode")]
        public Mode Mode { get; set; } = 0;
        
        [FromQuery(Name = "page")]
        public int Page { get; set; } = 0;

        public bool HasPreviousPage { get; set; } = true;
        public bool HasNextPage { get; set; } = true;
        
        public async Task<IActionResult> OnGet()
        {
            Page = Math.Clamp(Page - 1, 0, int.MaxValue);
            
            await LoadStats(Page, Mode, SearchQuery);

            return Page();
        }

        private async Task LoadStats(int page, Mode mode, string? searchQuery)
        {
            await using var database = new Database();

            if (page == 0)
                HasPreviousPage = false;

            var statsQuery = database.StatsWithRank
                .Include(s => s.User)
                .OrderBy(s => s.Rank)
                .Where(s => s.Mode == mode);

            if (!string.IsNullOrEmpty(searchQuery))
                statsQuery = statsQuery.Where(s => s.User.Username.Contains(searchQuery));

            int count = await statsQuery.CountAsync();

            if (PerPageRanks * (Page+1) > count)
                HasNextPage = false;
            
            statsQuery = statsQuery.Skip(page * PerPageRanks).Take(PerPageRanks);

            Stats = await statsQuery.ToArrayAsync();
        }

        public Leaderboard(AuthenticationService authenticationService) : base(authenticationService)
        {
        }
    }
}