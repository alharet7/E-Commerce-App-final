using E_Commerce_App.Models.DTOs;

namespace E_Commerce_App.Models.ViewModels
{
    public class LoginCategoryVM
    {
        public LoginDTO Login { get; set; }
        public List<Category> categories { get; set; }
    }
}
