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
        public Mode Mode { get; set; }

        public List<Badge>? Badges { get; set; }

        public async Task<IActionResult> OnGet([FromRoute] uint userId, [FromRoute] string strMode = "") 
        {
            switch(strMode) 
            {
                 case "osu":
                     Mode = Mode.Standard;
                     break;

                 case "taiko":
                     Mode = Mode.Taiko;
                     break;
                     
                 case "ctb":
                     Mode = Mode.CatchTheBeat;
                     break;
                 
                 case "mania":
                     Mode = Mode.Mania;
                     break;
                 
                 default:
                     return Redirect(PathCorrection.Correct($"/u/{userId}/osu"));
            };
            
            await using Database database = new Database();

            UserInfo = await database.UserInfo
                .FindAsync(userId);
            
            if(UserInfo == null) 
                return NotFound();

            UserStats = database.StatsWithRank
                .Include(s => s.User)
                .FirstOrDefault(s => s.UserID == userId && s.Mode == Mode);
            
            UserPageInfo = await database.UserPages
                .FirstOrDefaultAsync(s => s.UserID == userId);

            Badges = await database.Badges
                .Where(b => b.UserID == userId)
                .ToListAsync();

            if (UserStats != null)
            {
                // retrieve scores
                TopScores = await database.HighScoresWithRank
                    .Include(s => s.Beatmap)
                    .ThenInclude(b => b.Beatmapset)
                    .Where(s => s.UserId == userId && s.Gamemode == (byte)Mode && s.Beatmap.Beatmapset.RankingStatus > RankingStatus.Pending)
                    .OrderByDescending(s => s.Score)
                    .Take(5)
                    .ToListAsync();
                
                /*RecentScores = await database.Scores
                    .Where(s => s.UserId == userId && s.Gamemode == (byte)Mode)
                    .Include(s => s.Beatmap)
                    .ThenInclude(b => b.Beatmapset)
                    .OrderByDescending(s => s.SubmittedAt)
                    .Take(5)
                    .ToListAsync();*/
                
                
                RankHistory = new List<RankHistory>();
            
                // dog 123
                var wtf = await database.RankHistory.Where(r => r.UserID == userId && r.Mode == Mode).ToArrayAsync();
            
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