using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Oldsu.Enums;
using Oldsu.Types;
using Oldsu.Web.Authentication;
using Oldsu.Web.Utils;
using UserPageInfo = Oldsu.Types.UserPage;

namespace Oldsu.Web.Pages {
    public class UserPage : BaseLayout
    {
        public UserInfo UserInfo { get; set; }
        public StatsWithRank? UserStats { get; set; }
        public UserPageInfo? UserPageInfo { get; set; }
        public List<RankHistory>? RankHistory { get; set; }
        public List<HighScoreWithRank>? TopScores { get; set; }
        public List<ScoreRow>? RecentScores { get; set; }
        
        public List<Badge>? Badges { get; set; }

        public async Task<IActionResult> OnGet([FromRoute] uint userId, [FromRoute] string strMode = "") 
        {
            Mode mode;
            switch(strMode) 
            {
                 case "osu":
                     mode = Mode.Standard;
                     break;

                 case "taiko":
                     mode = Mode.Taiko;
                     break;
                     
                 case "ctb":
                     mode = Mode.CatchTheBeat;
                     break;
                 
                 case "mania":
                     mode = Mode.Mania;
                     break;
                 
                 default:
                     return Redirect(PathCorrection.Correct($"/u/{userId}/osu"));
            };
            
            await using Database database = new Database();

            UserInfo = await database.UserInfo
                .FindAsync(userId);
            
            if(UserInfo == null || UserInfo.Banned) 
                return NotFound();

           //UserStats = database.StatsWithRank
           //  .Include(s => s.User)
           //   .FirstOrDefault(s => s.UserID == userId && s.Mode == mode);

           UserPageInfo = await database.UserPages
                .FirstOrDefaultAsync(s => s.UserID == userId);

            Badges = await database.Badges
                .Where(b => b.UserID == userId)
                .ToListAsync();

            if (UserStats != null)
            {
                // retrieve scores
                TopScores = await database.HighScoresWithRank
                    .Where(s => s.UserId == userId && s.Gamemode == (byte)mode)
                    .Include(s => s.Beatmap)
                    .ThenInclude(b => b.Beatmapset)
                    .OrderByDescending(s => s.Score)
                    .Take(5)
                    .ToListAsync();
                
                RecentScores = await database.Scores
                    .Where(s => s.UserId == userId && s.Gamemode == (byte)mode)
                    .Include(s => s.Beatmap)
                    .ThenInclude(b => b.Beatmapset)
                    .OrderByDescending(s => s.SubmittedAt)
                    .Take(5)
                    .ToListAsync();
                
                
                RankHistory = new List<RankHistory>();
            
                // dog 123
                var wtf = await database.RankHistory.Where(r => r.UserID == userId && r.Mode == mode).ToArrayAsync();
            
                RankHistory.AddRange(wtf);
                RankHistory.Add(new RankHistory {UserID = userId, Date = DateTime.Now, Rank = UserStats.Rank});
            }
            
            return this.Page();
        }

        public UserPage(AuthenticationService authenticationService) : base(authenticationService)
        {
        }
    }
}