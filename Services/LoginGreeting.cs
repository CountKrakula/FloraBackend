using FloraBackend.Models;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Identity;

namespace FloraBackend.Services
{
    public static class LoginGreeting
    {
        public const string FirstLoginKey = "Flora.FirstLogin";
        private const string LoginHistoryProvider = "Flora";
        private const string LoginHistoryName = "HasSignedIn";

        public static async Task OnSigningInAsync(CookieSigningInContext context)
        {
            if (!HttpMethods.IsPost(context.Request.Method) ||
                !string.Equals(context.Request.Path.Value?.TrimEnd('/'), "/login", StringComparison.OrdinalIgnoreCase) ||
                context.Principal is null)
            {
                return;
            }

            var userManager = context.HttpContext.RequestServices.GetRequiredService<UserManager<User>>();
            var user = await userManager.GetUserAsync(context.Principal);
            if (user is null ||
                await userManager.GetAuthenticationTokenAsync(user, LoginHistoryProvider, LoginHistoryName) is not null)
            {
                return;
            }

            var result = await userManager.SetAuthenticationTokenAsync(user, LoginHistoryProvider, LoginHistoryName, "true");
            if (result.Succeeded)
            {
                context.Properties.Items[FirstLoginKey] = "true";
            }
        }
    }
}
