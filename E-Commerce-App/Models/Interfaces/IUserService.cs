using E_Commerce_App.Models.DTOs;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using System.Security.Claims;

namespace E_Commerce_App.Models.Interfaces
{
    public interface IUserService
    { //Register Method
        public Task<UserDTO> Register(RegisterUserDTO registerUserDTO, ModelStateDictionary modelstate);
        //login Method

        public Task<UserDTO> Authenticate(string username, string password);
        // Get All users method
        public Task<UserDTO> GetUser(ClaimsPrincipal principal);
        // logout method
        public Task LogOut();
        public Task<List<ApplicationUser>> getAll();
    }
}
