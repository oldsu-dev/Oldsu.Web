using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Oldsu.Types;
using Oldsu.Web.Utils;
using UserPageInfo = Oldsu.Types.UserPage;

namespace Oldsu.Web.Pages {
    public class UserPage : BaseLayout
    {
        public StatsWithRank UserStats { get; set; }
        public UserPageInfo UserPageInfo { get; set; }
        public List<RankHistory> RankHistory { get; set; }

        public async Task<IActionResult> OnGet([FromRoute] uint userId = 0) {
            
            await using Database database = new();

            StatsWithRank? userStats = database.StatsWithRank
                .Include(s => s.User)
                .FirstOrDefault(s => s.UserID == userId);

            
            if(userStats == null) return this.NotFound();
            UserStats = userStats;

            UserPageInfo = database.UserPages.FirstOrDefault(s => s.UserID == userId) ?? new UserPageInfo {
                UserID = userId,
                Birthday = null,
                Discord = string.Empty,
                Interests = string.Empty,
                Occupation = string.Empty,
                Twitter = string.Empty,
                Website = string.Empty
            };

            RankHistory = new List<RankHistory>();

            var wtf = await database.RankHistory.Where(r => r.UserID == userId).ToArrayAsync();
            
            RankHistory.AddRange(wtf);
            RankHistory.Add(new RankHistory {UserID = userId, Date = DateTime.Now, Rank = userStats.Rank});
            
            return this.Page();
        }
    }
}