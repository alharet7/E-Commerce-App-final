using E_Commerce_App.Models;
using E_Commerce_App.Models.DTOs;
using E_Commerce_App.Models.Interfaces;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace E_Commerce_App.Controllers
{
    /// <summary>
    /// Controller responsible for handling home-related actions.
    /// </summary>
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IAddImageToCloud _addImageToCloud;
        private readonly ICategory _categoryService;
        private readonly IEmail _email;

        /// <summary>
        /// Initializes a new instance of the <see cref="HomeController"/> class.
        /// </summary>
        /// <param name="logger">The logger.</param>
        /// <param name="add">Image upload service.</param>
        /// <param name="category">Category service.</param>
        public HomeController(ILogger<HomeController> logger, IAddImageToCloud add, ICategory category, IEmail email)
        {
            _logger = logger;
            _addImageToCloud = add;
            _categoryService = category;
            _email = email;
        }

        /// <summary>
        /// Displays the home page with a list of categories.
        /// </summary>
        /// <returns>The home page view.</returns>
        public async Task<IActionResult> Index()
        {
            var categories = await _categoryService.GetAllCategories();

            if (User.Identity.IsAuthenticated)
            {
                var model = new LoginDTO { UserName = User.Identity.Name };
                ViewData["Categories"] = categories;
                return View(model);
            }
            else
            {
                ViewData["Categories"] = categories;
                return View();
            }
        }

        /// <summary>
        /// Displays the privacy page.
        /// </summary>
        /// <returns>The privacy page view.</returns>
        public IActionResult Privacy()
        {
            return View();
        }

        /// <summary>
        /// Displays the error page with error details.
        /// </summary>
        /// <returns>The error page view.</returns>
        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
