using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Oldsu.Web.Models;

namespace Oldsu.Web.Pages
{
    [IgnoreAntiforgeryToken]
    public class Register : BaseLayout
    {
        public string RegistrationResult { get; set; }

        public void OnGet()
        {
           Console.WriteLine("received get");
           RegistrationResult = "joo";
        }
        
        public async Task OnPost(RegisterSubmitModel registerData)
        {
            await using var database = new Database();
            
            //database.RegisterAsync()
        }
    }
}