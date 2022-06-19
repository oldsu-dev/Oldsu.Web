using Microsoft.AspNetCore.Mvc.RazorPages;
using Oldsu.Web.Authentication;

namespace Oldsu.Web.Pages;

public class Copyright : BaseLayout
{
    public void OnGet()
    {
        
    }

    public Copyright(AuthenticationService authenticationService) : base(authenticationService)
    {
    }
}