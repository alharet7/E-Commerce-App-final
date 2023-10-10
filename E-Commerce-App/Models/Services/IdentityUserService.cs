using E_Commerce_App.Models.DTOs;
using E_Commerce_App.Models.Interfaces;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace E_Commerce_App.Models.Services
{
    /// <summary>
    /// Service class for managing user operations using ASP.NET Identity.
    /// </summary>
    public class IdentityUserService : IUserService
    {
        private readonly UserManager<ApplicationUser> _userManager;

        private readonly SignInManager<ApplicationUser> _signInManager;

        //private readonly JWTTokenService _jwtTokenService;

        /// <summary>
        /// Initializes a new instance of the <see cref="IdentityUserService"/> class.
        /// </summary>
        /// <param name="userManager">The UserManager for user management.</param>
        /// <param name="SignInMngr">The SignInManager for user sign-in.</param>

        public IdentityUserService(UserManager<ApplicationUser> userManager, SignInManager<ApplicationUser> SignInMngr)//, JWTTokenService jwtTokenService )
        {
            _userManager = userManager;
            _signInManager = SignInMngr;
            // _jwtTokenService = jwtTokenService;
        }

        /// <summary>
        /// Registers a new user.
        /// </summary>
        /// <param name="registerUserDTO">The DTO containing user registration information.</param>
        /// <param name="modelState">The ModelStateDictionary to collect validation errors.</param>
        /// <returns>A UserDTO representing the registered user if successful, null otherwise.</returns>

        public async Task<UserDTO> Register(RegisterUserDTO registerUserDTO, ModelStateDictionary modelState)
        {
            var user = new ApplicationUser()
            {
                UserName = registerUserDTO.UserName,
                Email = registerUserDTO.Email,
                PhoneNumber = registerUserDTO.PhoneNumber,
            };

            var result = await _userManager.CreateAsync(user, registerUserDTO.Password);
            if (result.Succeeded)
            {
                await _userManager.AddToRolesAsync(user, registerUserDTO.Roles);

                return new UserDTO()
                {
                    Id = user.Id,
                    UserName = user.UserName,
                    //Token = await _jwtTokenService.GetToken(user, System.TimeSpan.FromMinutes(60)),
                    Roles = await _userManager.GetRolesAsync(user)
                };

            }
            foreach (var error in result.Errors)
            {
                var errorKey = error.Code.Contains("Password") ? nameof(registerUserDTO.Password) :
                error.Code.Contains("UserName") ? nameof(registerUserDTO.UserName) :
                     error.Code.Contains("Email") ? nameof(registerUserDTO.Email) :
                     "";

                modelState.AddModelError(errorKey, error.Description);
            }

            return null;
        }

        /// <summary>
        /// Authenticates a user.
        /// </summary>
        /// <param name="userName">The username of the user to authenticate.</param>
        /// <param name="password">The user's password.</param>
        /// <returns>A UserDTO representing the authenticated user if successful, null otherwise.</returns>

        public async Task<UserDTO> Authenticate(string userName, string password)
        {
            var result = await _signInManager.PasswordSignInAsync(userName, password, true, false);
            if (result.Succeeded)
            {
                var user = await _userManager.FindByNameAsync(userName);
                return new UserDTO()
                {
                    Id = user.Id,
                    UserName = user.UserName,
                    // Token = await _jwtTokenService.GetToken(user, System.TimeSpan.FromMinutes(60)),
                    Roles = await _userManager.GetRolesAsync(user)
                };
            }
            return null;
        }

        /// <summary>
        /// Gets a list of all users.
        /// </summary>
        /// <returns>A list of ApplicationUser objects.</returns>
        public async Task<List<ApplicationUser>> getAll()
        {
            return await _userManager.Users.ToListAsync();
        }

        /// <summary>
        /// Gets a UserDTO based on the ClaimsPrincipal.
        /// </summary>
        /// <param name="principal">The ClaimsPrincipal representing the user.</param>
        /// <returns>A UserDTO representing the user.</returns>

        public async Task<UserDTO> GetUser(ClaimsPrincipal principal)
        {
            var user = await _userManager.GetUserAsync(principal);
            return new UserDTO
            {
                UserName = user.UserName
            };
        }
        /// <summary>
        /// Logs the user out.
        /// </summary>
        /// <returns>A task representing the logout operation.</returns>

        public async Task LogOut()
        {
            await _signInManager.SignOutAsync();
        }

    }
}
