using System;
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

        public async Task<IActionResult> OnGet([FromRoute] int userId = -1) {
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

            return this.Page();
        }
    }
}