using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Primitives;
using Oldsu.DatabaseServices;
using Oldsu.DatabaseServices.MySql;
using Oldsu.Enums;
using Oldsu.Types;
using Oldsu.Web.Authentication;

namespace Oldsu.Web.Pages
{
    public class Leaderboard : BaseLayout
    {
        private const int PerPageRanks = 50;

        public IStatService MySqlStatService;

        [FromQuery(Name = "query")] 
        public string? SearchQuery { get; set; } = string.Empty;

        [FromQuery(Name = "mode")]
        public Mode Mode { get; set; } = 0;
        
        [FromQuery(Name = "page")]
        public int Page { get; set; } = 0;
        
        public async Task<IActionResult> OnGet()
        {
            Page = Math.Clamp(Page - 1, 0, int.MaxValue);
            
            await LoadStats(Page, Mode, SearchQuery);
            
            return Page();
        }
        
        public StatsWithRank?[] Stats { get; private set; }
        public DbSet<StatsWithRank?> Users;
        
        private async Task LoadStats(int page, Mode mode, string? searchQuery)
        {
            await using var database = new Database();
            foreach (UserInfo user in database.UserInfo)
            {
                Users.Add(await MySqlStatService.GetStatsWithRankAsync(user.UserID, Mode.Standard));
            }
            
            var statsQuery = Users
                .Include(s => s!.User)
                .OrderBy(s => s!.Rank)
                .Where(s => s!.Mode == mode); 

            if (!string.IsNullOrEmpty(searchQuery))
                statsQuery = statsQuery.Where(s => s.User.Username.Contains(searchQuery));

            statsQuery = statsQuery.Skip(page * PerPageRanks).Take(PerPageRanks);
        
            //Stats = await statsQuery.ToArrayAsync();
            Stats = statsQuery.ToArray(); 

        }

        public Leaderboard(AuthenticationService authenticationService, IStatService mySqlStatService) : base(authenticationService)
        {
            MySqlStatService = mySqlStatService;
        }
    }
}