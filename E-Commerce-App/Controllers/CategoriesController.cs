using E_Commerce_App.Data;
using E_Commerce_App.Models;
using E_Commerce_App.Models.Interfaces;
using E_Commerce_App.Models.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace E_Commerce_App.Controllers
{
    /// <summary>
    /// Controller responsible for managing categories and related actions.
    /// </summary>
    public class CategoriesController : Controller
    {
        private readonly StoreDbContext _context;
        private readonly ICategory _category;
        private readonly IProduct _product;
        private readonly IAddImageToCloud _addImageToCloud;

        /// <summary>
        /// Initializes a new instance of the <see cref="CategoriesController"/> class.
        /// </summary>
        /// <param name="category">Category service.</param>
        /// <param name="product">Product service.</param>
        /// <param name="addImageToCloud">Image upload service.</param>
        /// <param name="context">Database context.</param>
        public CategoriesController(ICategory category, IProduct product, IAddImageToCloud addImageToCloud, StoreDbContext context)
        {
            _category = category;
            _product = product;
            _addImageToCloud = addImageToCloud;
            _context = context;
        }

        /// <summary>
        /// Displays a list of all categories.
        /// </summary>
        /// <returns>The category list view.</returns>
        public async Task<IActionResult> Index()
        {
            var categories = await _category.GetAllCategories();
            return categories != null ? View(categories) : Problem("Entity set 'StoreDbContext.Categories' is null.");
        }

        /// <summary>
        /// Displays details of a specific category and its associated products.
        /// </summary>
        /// <param name="categoryID">The ID of the category to display.</param>
        /// <returns>The category details view.</returns>
        public async Task<IActionResult> CategoryDetails(int categoryID)
        {
            var category = await _category.GetCategoryById(categoryID);

            var products = await _product.GetProductsByCategory(categoryID);

            CategoryProductVM productsByCategory = new CategoryProductVM()
            {
                Category = category,
                Products = products
            };
            return View(productsByCategory);
        }

        /// <summary>
        /// Displays details of a specific category.
        /// </summary>
        /// <param name="id">The ID of the category to display.</param>
        /// <returns>The category details view.</returns>
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var category = await _category.GetCategoryById(id.Value);
            if (category == null)
            {
                return NotFound();
            }

            return View(category);
        }

        /// <summary>
        /// Displays the create category form.
        /// </summary>
        /// <returns>The create category view.</returns>

        [Authorize(Roles = "Administrator")]
        public IActionResult Create()
        {
            return View();
        }

        /// <summary>
        /// Handles the creation of a new category.
        /// </summary>
        /// <param name="category">The category data to create.</param>
        /// <param name="file">The image file associated with the category.</param>
        /// <returns>Redirects to the category list if successful; otherwise, returns the create category view.</returns>
        [Authorize(Roles = "Administrator")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("CategoryId,Name,imgURL")] Category category, IFormFile file)
        {
            if (file != null)
                await _addImageToCloud.UploadCategoryImage(file, category);
            else
            {
                ModelState.Remove("file");
                category.imgURL = "https://lab29ecommerceimages.blob.core.windows.net/projectimages/default-image.jpg";
            }

            if (ModelState.IsValid)
            {
                await _category.CreateNewCategory(category);
                return RedirectToAction(nameof(Index));
            }
            return View(category);
        }

        /// <summary>
        /// Displays the edit category form for a specific category.
        /// </summary>
        /// <param name="id">The ID of the category to edit.</param>
        /// <returns>The edit category view.</returns>
        [Authorize(Roles = "Editor")]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var category = await _category.GetCategoryById(id.Value);
            if (category == null)
            {
                return NotFound();
            }
            return View(category);
        }

        /// <summary>
        /// Handles the editing of a category.
        /// </summary>
        /// <param name="id">The ID of the category to edit.</param>
        /// <param name="category">The updated category data.</param>
        /// <param name="file">The updated image file associated with the category.</param>
        /// <returns>Redirects to the category list if successful; otherwise, returns the edit category view.</returns>
        [Authorize(Roles = "Editor")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, Category category, IFormFile file)
        {
            if (id != category.CategoryId)
            {
                return NotFound();
            }

            var oldCategory = await _category.GetCategoryById(id);

            if (file != null)
            {
                await _addImageToCloud.UploadCategoryImage(file, category);
            }
            else
            {
                ModelState.Remove("file");
            }

            if (ModelState.IsValid)
            {
                try
                {
                    await _category.UpdateCategory(id, category);
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!await CategoryExists(category.CategoryId))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            return View(category);
        }

        /// <summary>
        /// Displays the delete category confirmation page.
        /// </summary>
        /// <param name="id">The ID of the category to delete.</param>
        /// <returns>The delete category view.</returns>

        [Authorize(Roles = "Administrator")]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var category = await _category.GetCategoryById(id.Value);
            if (category == null)
            {
                return NotFound();
            }

            return View(category);
        }

        /// <summary>
        /// Handles the deletion of a category.
        /// </summary>
        /// <param name="id">The ID of the category to delete.</param>
        /// <returns>Redirects to the category list if successful.</returns>
        [Authorize(Roles = "Administrator")]
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            await _category.DeleteCategory(id);
            return RedirectToAction(nameof(Index));
        }

        private async Task<bool> CategoryExists(int id)
        {
            var category = await _category.GetCategoryById(id);
            return category != null;
        }
    }
}
