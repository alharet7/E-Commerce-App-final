using E_Commerce_App.Models;
using Microsoft.AspNetCore.Identity;
using System.Security.Claims;

namespace E_Commerce_App.Views.Shared
{
    public static class SharedFunctions
    {
        public static bool StringIsNullOrEmpty(string obj)
        {
            if (obj != null && obj != string.Empty)
            {
                return false;
            }
            return true;
        }

        public static bool IsAuthenticated(SignInManager<ApplicationUser> signInManager, ClaimsPrincipal user)
        {
            if (signInManager.IsSignedIn(user))
            {
                return true;
            }
            else
                return false;
        }
    }
}
