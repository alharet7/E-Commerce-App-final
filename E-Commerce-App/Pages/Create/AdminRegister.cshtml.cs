using E_Commerce_App.Models.DTOs;
using E_Commerce_App.Models.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace E_Commerce_App.Pages
{
    public class SignUpModel : PageModel
    {
        private readonly IUserService _userService;

        [BindProperty]
        public RegisterUserDTO Register { get; set; }

        [BindProperty]
        public string UserRole { get; set; }

        public SignUpModel(IUserService userService)
        {
            _userService = userService;
        }

        public IActionResult OnGet()
        {
            if (!User.Identity.IsAuthenticated || !User.IsInRole("Administrator"))
            {
                return Unauthorized();
            }

            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!User.Identity.IsAuthenticated || !User.IsInRole("Administrator"))
            {
                return Unauthorized();
            }

            if (UserRole == "Administrator")
                Register.Roles = new List<string>() { "Administrator" };
            else if (UserRole == "Editor")
                Register.Roles = new List<string>() { "Editor" };
            else
                return Page();

            var user = await _userService.Register(Register, this.ModelState);

            // generate file with user information
            if (user != null)
            {
                string fileName = Register.UserName + " Login Information.txt";
                using (StreamWriter writer = System.IO.File.CreateText(fileName))
                {
                    writer.WriteLine($"Name: {Register.UserName}");
                    writer.WriteLine($"Email: {Register.Email}");
                    writer.WriteLine($"PhoneNumber: {Register.PhoneNumber}");
                    writer.WriteLine($"Role: {Register.Roles.FirstOrDefault()}");
                    // Add more properties as needed
                }

                // Download the file
                var memory = new MemoryStream();
                using (var stream = new FileStream(fileName, FileMode.Open))
                {
                    await stream.CopyToAsync(memory);
                }
                memory.Position = 0;
                return File(memory, "text/plain", Path.GetFileName(fileName));
            }
            return Page();
        }
    }
}
