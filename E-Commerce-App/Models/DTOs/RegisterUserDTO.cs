using System.ComponentModel.DataAnnotations;

namespace E_Commerce_App.Models.DTOs
{
    public class RegisterUserDTO
    {
        [Required(ErrorMessage = "You have missed to fill the username")]
        [Display(Name = "User Name")]
        [MinLength(3)]
        public string UserName { get; set; }
        [Required]
        public string Password { get; set; }
        [Required]
        public string Email { get; set; }
        [Required]
        [Display(Name = "Phone Number")]
        public string PhoneNumber { get; set; }

        public IList<string>? Roles { get; set; }
    }
}
