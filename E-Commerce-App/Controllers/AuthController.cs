using E_Commerce_App.Models;
using E_Commerce_App.Models.DTOs;
using E_Commerce_App.Models.Interfaces;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace E_Commerce_App.Controllers
{
    /// <summary>
    /// Controller responsible for authentication and user-related actions.
    /// </summary>
    public class AuthController : Controller
    {
        private IUserService userService;
        private readonly IEmail _email;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly UserManager<ApplicationUser> _userManager;

        /// <summary>
        /// Initializes a new instance of the <see cref="AuthController"/> class.
        /// </summary>
        /// <param name="service">User service.</param>
        /// <param name="userManager">User manager.</param>
        /// <param name="signInManager">Sign-in manager.</param>
        public AuthController(IUserService service, UserManager<ApplicationUser> userManager, SignInManager<ApplicationUser> signInManager, IEmail email)
        {
            userService = service;
            _userManager = userManager;
            _signInManager = signInManager;
            _email = email;
        }

        /// <summary>
        /// Displays the default view for authentication.
        /// </summary>
        /// <returns>The default authentication view.</returns>
        public IActionResult Index()
        {
            return View();
        }

        /// <summary>
        /// Displays the view for user registration.
        /// </summary>
        /// <returns>The registration view.</returns>
        public IActionResult Signup()
        {
            return View();
        }

        /// <summary>
        /// Handles user registration when the registration form is submitted.
        /// </summary>
        /// <param name="register">User registration data.</param>
        /// <returns>
        ///   <list type="bullet">
        ///     <item>
        ///       <description>If registration is successful, redirects to the "Home/Index" action.</description>
        ///     </item>
        ///     <item>
        ///       <description>If registration fails, returns the registration view with validation errors.</description>
        ///     </item>
        ///   </list>
        /// </returns>

        [HttpPost]
        public async Task<ActionResult<UserDTO>> SignUp(RegisterUserDTO register)
        {
            register.Roles = new List<string>() { "Users" };

            var user = await userService.Register(register, this.ModelState);
            if (ModelState.IsValid)
            {
                await userService.Authenticate(register.UserName, register.Password);

                string emailSubject = "Welcome";

                string emailBody = "<!DOCTYPE html>\r\n<html>\r\n<head>\r\n" +
                    "<title>Welcome to Tech Pioneers</title>\r\n" +
                    "<style>\r\n" +
                    "body {\r\n" +
                    "font-family: Arial, sans-serif;\r\n" +
                    "margin: 0;\r\n" +
                    "padding: 0;\r\n" +
                    "background-color: #f4f4f4;\r\n" +
                    "}\r\n" +
                    ".email-container {\r\n" +
                    "width: 80%;\r\n" +
                    "margin: auto;\r\n" +
                    "padding: 20px;\r\n" +
                    "background-color: #fff;\r\n" +
                    "box-shadow: 0px 0px 20px 0px rgba(0,0,0,0.1);\r\n " +
                    "}\r\n" +
                    "h1 {\r\n" +
                    "color: #444;\r\n" +
                    "}\r\n" +
                    "p {\r\n" +
                    "color: #666;\r\n" +
                    "line-height: 1.6;\r\n" +
                    "}\r\n" +
                    "</style>\r\n</head>\r\n<body>\r\n  " +
                    "<div class=\"email-container\">\r\n" +
                    "<h1>Welcome to Tech Pioneers!</h1>\r\n" +
                    $"<p>Dear {register.UserName},</p>\r\n" +
                    "<p>We're excited to have you on board. Thank you for registering at our e-commerce application, Tech Pioneers.</p>\r\n " +
                    "<p>Start exploring the latest tech products on our platform. We're sure you'll find something you love!</p>\r\n " +
                    "<p>If you have any questions or need assistance, feel free to reach out to our support team.</p>\r\n" +
                    "<p>Happy shopping!</p>\r\n" +
                    "<p>Best,</p>\r\n" +
                    "<p>The Tech Pioneers Team</p>\r\n" +
                    "</div>\r\n</body>\r\n</html>\r\n";

                _email.SendEmail(register.Email, register.UserName, emailSubject, emailBody);

                return Redirect("/Home/Index");
            }
            else
            {
                return View(register);
            }
        }

        /// <summary>
        /// Handles user authentication when the login form is submitted.
        /// </summary>
        /// <param name="loginData">User login data.</param>
        /// <returns>
        ///   <list type="bullet">
        ///     <item>
        ///       <description>If authentication is successful, sets a welcome message and redirects to the "Home/Index" action.</description>
        ///     </item>
        ///     <item>
        ///       <description>If authentication fails, returns the "Index" view with an error message.</description>
        ///     </item>
        ///   </list>
        /// </returns>
        [HttpPost]
        public async Task<ActionResult<UserDTO>> Authenticate(LoginDTO loginData)
        {
            var user = await userService.Authenticate(loginData.UserName, loginData.Password);

            if (user == null)
            {
                ModelState.AddModelError(string.Empty, "Username or password is incorrect.");
                return View("Index", loginData);
            }
            else
            {
                TempData["AlertMessage"] = $"Welcome {loginData.UserName} in Tech Pioneers Website :)";
                return RedirectToAction("Index", "Home");
            }
        }

        /// <summary>
        /// Logs the user out and redirects to the "Home/Index" action.
        /// </summary>
        /// <returns>Redirects to the "Home/Index" action.</returns>
        public async Task<IActionResult> LogOut()
        {
            await _signInManager.SignOutAsync();
            if (Request.Cookies["productIds"] != null)
            {
                Response.Cookies.Delete("productIds");
            }
            return RedirectToAction("Index", "Home");
        }
    }
}
