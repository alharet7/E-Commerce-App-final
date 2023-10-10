using E_Commerce_App.Models;
using E_Commerce_App.Models.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace E_Commerce_App.Controllers
{
    /// <summary>
    /// Controller responsible for managing products and related actions.
    /// </summary>
    public class ProductsController : Controller
    {
        private readonly IProduct _product;
        private readonly IAddImageToCloud _addImageToCloud;
        private readonly ICategory _category;

        /// <summary>
        /// Initializes a new instance of the <see cref="ProductsController"/> class.
        /// </summary>
        /// <param name="context">The database context.</param>
        /// <param name="product">Product service.</param>
        /// <param name="addImageToCloud">Image upload service.</param>
        /// <param name="category">Category service.</param>
        public ProductsController(IProduct product, IAddImageToCloud addImageToCloud, ICategory category)
        {
            _product = product;
            _addImageToCloud = addImageToCloud;
            _category = category;
        }

        /// <summary>
        /// Displays a list of all products.
        /// </summary>
        /// <returns>The product list view.</returns>
        public async Task<IActionResult> Index()
        {
            var returnList = await _product.GetAllProducts();
            return View(returnList);
        }

        /// <summary>
        /// Displays products based on a category.
        /// </summary>
        /// <param name="categoryID">The ID of the category to filter products by.</param>
        /// <returns>The product list view filtered by category.</returns>
        public async Task<IActionResult> GetProducts(int categoryID)
        {
            var products = await _product.GetProductsByCategory(categoryID);
            return View(products);
        }

        /// <summary>
        /// Displays product details for a specific product.
        /// </summary>
        /// <param name="productID">The ID of the product to display.</param>
        /// <returns>The product details view.</returns>
        public async Task<IActionResult> ProductDetails(int productID)
        {
            var productDetails = await _product.GetProductById(productID);
            return View(productDetails);
        }

        /// <summary>
        /// Displays details of a specific product.
        /// </summary>
        /// <param name="id">The ID of the product to display.</param>
        /// <returns>The product details view.</returns>
        public async Task<IActionResult> Details(int id)
        {
            if (id == null || await _product.GetAllProducts() == null)
            {
                return NotFound();
            }

            var product = await _product.GetProductById(id);
            if (product == null)
            {
                return NotFound();
            }

            return View(product);
        }

        /// <summary>
        /// Displays the create product form.
        /// </summary>
        /// <returns>The create product view.</returns>
        [Authorize(Roles = "Administrator")]
        public async Task<IActionResult> Create()
        {
            ViewData["CategoryId"] = new SelectList(await _category.GetAllCategories(), "CategoryId", "Name");
            return View();
        }

        /// <summary>
        /// Handles the creation of a new product.
        /// </summary>
        /// <param name="file">The image file associated with the product.</param>
        /// <param name="product">The product data to create.</param>
        /// <returns>Redirects to the product list if successful; otherwise, returns the create product view.</returns>
        [Authorize(Roles = "Administrator")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(IFormFile file, [Bind("ProductId,CategoryId,Name,Description,Price,StockQuantity,ProductImage")] Product product)
        {
            if (file != null)
                await _addImageToCloud.UploadProductImage(file, product);
            else
            {
                ModelState.Remove("file");
                product.ProductImage = "https://lab29ecommerceimages.blob.core.windows.net/projectimages/default-image.jpg";
            }
            if (ModelState.IsValid)
            {
                await _product.AddNewProduct(file, product);
                return RedirectToAction(nameof(Index));
            }
            ViewData["CategoryId"] = new SelectList(await _category.GetAllCategories(), "CategoryId", "CategoryId", product.CategoryId);
            return View(product);
        }

        /// <summary>
        /// Displays the edit product form for a specific product.
        /// </summary>
        /// <param name="id">The ID of the product to edit.</param>
        /// <returns>The edit product view.</returns>
        [Authorize(Roles = "Editor")]
        public async Task<IActionResult> Edit(int id)
        {
            TempData["referrerUrl"] = Request.Headers["Referer"].ToString();

            if (id == null || await _product.GetAllProducts() == null)
            {
                return NotFound();
            }

            var product = await _product.GetProductById(id);
            if (product == null)
            {
                return NotFound();
            }
            ViewData["CategoryId"] = new SelectList(await _category.GetAllCategories(), "CategoryId", "Name", product.CategoryId);
            return View(product);
        }

        /// <summary>
        /// Handles the editing of a product.
        /// </summary>
        /// <param name="id">The ID of the product to edit.</param>
        /// <param name="product">The updated product data.</param>
        /// <param name="file">The updated image file associated with the product.</param>
        /// <returns>Redirects to the previous page if successful; otherwise, returns the edit product view.</returns>
        [Authorize(Roles = "Editor")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, Product product, IFormFile file)
        {
            if (id != product.ProductId)
            {
                return NotFound();
            }

            var oldProduct = await _product.GetProductById(id);

            if (file != null)
            {
                // If a new file is uploaded, update the ProductImage property
                oldProduct = await _addImageToCloud.UploadProductImage(file, product);
            }
            else
            {
                ModelState.Remove("file");
            }

            if (ModelState.IsValid)
            {
                try
                {
                    await _product.UpdateProduct(id, product);
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (await ProductExists(product.ProductId) == false)
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                string referrerUrl = TempData["referrerUrl"] as string;
                return Redirect(referrerUrl);
            }
            ViewData["CategoryId"] = new SelectList(await _category.GetAllCategories(), "CategoryId", "CategoryId", product.CategoryId);
            return View(product);
        }

        /// <summary>
        /// Displays the delete product confirmation page.
        /// </summary>
        /// <param name="id">The ID of the product to delete.</param>
        /// <returns>The delete product view.</returns>
        [Authorize(Roles = "Administrator")]
        public async Task<IActionResult> Delete(int id)
        {
            TempData["referrerUrl"] = Request.Headers["Referer"].ToString();

            if (id == null || await _product.GetAllProducts() == null)
            {
                return NotFound();
            }

            var product = await _product.GetProductById(id);
            if (product == null)
            {
                return NotFound();
            }

            return View(product);
        }

        /// <summary>
        /// Handles the deletion of a product.
        /// </summary>
        /// <param name="id">The ID of the product to delete.</param>
        /// <returns>Redirects to the previous page if successful.</returns>
        [Authorize(Roles = "Administrator")]
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (await _product.GetAllProducts() == null)
            {
                return Problem("Entity set 'StoreDbContext.Products' is null.");
            }

            await _product.DeleteProduct(id);

            string referrerUrl = TempData["referrerUrl"] as string;
            return Redirect(referrerUrl);
        }

        private async Task<bool> ProductExists(int id)
        {
            if (await _product.GetProductById(id) == null)
            {
                return false;
            }
            return true;
        }

        /// <summary>
        /// Searches for products based on a query string.
        /// </summary>
        /// <param name="query">The search query.</param>
        /// <returns>The search results view.</returns>
        public async Task<IActionResult> Search(string query)
        {
            var products = await _product.Search(query);

            return View(products);
        }
    }
}
