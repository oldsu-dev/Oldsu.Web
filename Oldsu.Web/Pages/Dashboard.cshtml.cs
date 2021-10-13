using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Oldsu.Web.Pages
{
    public class Dashboard : BaseLayout
    {
        public Types.UserPage UserPageInformation { get; set; }

        public async Task OnGet() { }
    }
}