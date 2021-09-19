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
        public UserInfo? UserInfo { get; set; }

        public StatsWithRank? UserStats { get; set; }
        public UserPageInfo? UserPageInfo { get; set; }
        public IAsyncEnumerable<ScoreRow> Scores { get; set; }

        public async Task<IActionResult> OnGet([FromRoute] uint userId)
        {
            if (userId == null)
                return NotFound();
            
            await using var database = new Database();

            UserInfo = await database.UserInfo
                .FindAsync(userId);
            
            UserStats = await database.StatsWithRank
                .Include(s => s.User)
                .FirstOrDefaultAsync(s => s.UserID == userId);
            
            UserPageInfo = await database.UserPages
                .FirstOrDefaultAsync(s => s.UserID == userId);

            if(UserInfo == null) 
                return NotFound();

            return Page();
        }
    }
}