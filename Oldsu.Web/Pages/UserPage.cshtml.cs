using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Oldsu.Types;

namespace Oldsu.Web.Pages {
    public class UserPage : BaseLayout {
        public StatsWithRank UserStats { get; set; }

        public async Task<IActionResult> OnGet([FromRoute] int userId = -1) {
            await using Database database = new();

            StatsWithRank? userStats = null;

            try {
                userStats = database.StatsWithRank
                    .Include(s => s.User)
                    .First(s => s.UserID == userId);
            }
            catch(InvalidOperationException) {
                // no HResult for "Sequence contains no elements", just assume that's the error for now and move on
            }

            if(userStats == null) return this.NotFound();
            UserStats = userStats;

            return this.Page();
        }
    }
}