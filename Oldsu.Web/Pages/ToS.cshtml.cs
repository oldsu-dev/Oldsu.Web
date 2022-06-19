using Microsoft.AspNetCore.Mvc.RazorPages;
using Oldsu.Web.Authentication;

namespace Oldsu.Web.Pages;

public class ToS : BaseLayout
{
    public ToS(AuthenticationService authenticationService) : base(authenticationService)
    {
    }
}