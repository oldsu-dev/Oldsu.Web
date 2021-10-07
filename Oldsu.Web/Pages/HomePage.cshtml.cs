using System;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics;
using System.Linq;
using System.Security.Cryptography;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using Oldsu.Types;
using Oldsu.Web.Models;
using Oldsu.Web.Validators;

namespace Oldsu.Web.Pages
{
    [IgnoreAntiforgeryToken]
    public class HomePage : BaseLayout
    {
        public News? LatestNews { get; private set; }
    
        public async Task<IActionResult> OnGet()
        {
            await using var database = new Database();
            LatestNews = await database.News.OrderByDescending(n => n.Date).FirstOrDefaultAsync();
            
            return Page();
        }

        const string characters = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ1234567890";        
        
        // Login requests go here..?
        public async Task OnPost([FromForm] LoginSubmitModel loginData)
        {
            if (loginData.Password == null || loginData.Username == null)
            {
                return;
            }

            if (loginData.Password.Length != 64)
            {
                return;
            }

            await using var db = new Database();

            var user = await db.AuthenticateAsync(loginData.Username, loginData.Password);

            if (user == null)
                return;

            var sessionId = $"{Guid.NewGuid().ToString()}{DateTime.Now.Ticks}";

            // todo move to sessionidprovider class
            using (var rng = new RNGCryptoServiceProvider())
            {
                int count = (int)Math.Ceiling(Math.Log(characters.Length, 2) / 8.0);
                Debug.Assert(count <= sizeof(uint));
                int offset = BitConverter.IsLittleEndian ? 0 : sizeof(uint) - count;
                int max = (int)(Math.Pow(2, count*8) / characters.Length) * characters.Length;
                byte[] uintBuffer = new byte[sizeof(uint)];

                while (sessionId.Length < 128)
                {
                    rng.GetBytes(uintBuffer, offset, count);
                    uint num = BitConverter.ToUInt32(uintBuffer, 0);
                    if (num < max)
                    {
                        sessionId += characters[(int) (num % characters.Length)];
                    }
                }
            }

            await db.AddWebSession(sessionId, user.UserID);
            
            Response.Cookies.Append("oldsu-sid", sessionId);
            
            Response.Redirect("/");
        }
    }
}