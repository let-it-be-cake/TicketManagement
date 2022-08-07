using System;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Localization;

namespace TicketManagement.UserInterface.Helper
{
    internal static class CookieHelper
    {
        public static void SetLocal(HttpResponse response, string culture)
        {
            response.Cookies.Append(
                   CookieRequestCultureProvider.DefaultCookieName,
                   CookieRequestCultureProvider.MakeCookieValue(new RequestCulture(culture)),
                   new CookieOptions
                   {
                       Expires = DateTimeOffset.UtcNow.AddYears(1),
                   });
        }
    }
}
