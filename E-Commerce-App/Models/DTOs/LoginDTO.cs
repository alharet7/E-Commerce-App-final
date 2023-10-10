using System.ComponentModel.DataAnnotations;

namespace E_Commerce_App.Models.DTOs
{
    public class LoginDTO
    {
        [Required]
        public string UserName { get; set; }
        [Required]
        public string Password { get; set; }
    }
}
