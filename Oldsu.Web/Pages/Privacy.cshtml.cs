using Microsoft.AspNetCore.Mvc.RazorPages;
using Oldsu.Web.Authentication;

namespace Oldsu.Web.Pages;

public class Privacy : BaseLayout
{
    public Privacy(AuthenticationService authenticationService) : base(authenticationService)
    {
    }
}