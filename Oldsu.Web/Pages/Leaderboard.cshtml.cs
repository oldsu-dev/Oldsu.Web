using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Primitives;
using Oldsu.Enums;
using Oldsu.Types;

namespace Oldsu.Web.Pages
{
    public class Leaderboard : BaseLayout
    {
        public StatsWithRank[] Stats { get; private set; } 
        
        public async Task<IActionResult> OnGet()
        {
            StringValues usernameQuery, gamemodeQuery;

            if (!Request.Query.TryGetValue("gamemode", out gamemodeQuery))
                gamemodeQuery = "0";

            if (!Request.Query.TryGetValue("username", out usernameQuery))
                usernameQuery = string.Empty;
            
            int gamemode = Math.Clamp(int.Parse(gamemodeQuery), 0, 2);
            
            await LoadStats(gamemode, usernameQuery);

            return Page();
        }

        private async Task LoadStats(int gamemode, StringValues usernameQuery)
        {
            await using var database = new Database();

            var statsQuery = database.StatsWithRank
                .Include(s => s.User)
                .OrderBy(s => s.Rank)
                .Where(s => s.Mode == (Mode) gamemode);

            if (usernameQuery == string.Empty)
                statsQuery = statsQuery.Where(s => s.User.Username.Contains(usernameQuery));

            Stats = await statsQuery.ToArrayAsync();
        }
    }
}