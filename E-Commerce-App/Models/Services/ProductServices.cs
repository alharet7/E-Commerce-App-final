using E_Commerce_App.Data;
using E_Commerce_App.Models.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace E_Commerce_App.Models.Services
{
    /// <summary>
    /// Service class for managing product-related operations.
    /// </summary>
    public class ProductServices : IProduct
    {
        private readonly StoreDbContext _context;

        /// <summary>
        /// Initializes a new instance of the <see cref="ProductServices"/> class.
        /// </summary>
        /// <param name="db">The StoreDbContext for accessing the database.</param>
        public ProductServices(StoreDbContext db)
        {
            _context = db;
        }

        /// <summary>
        /// Adds a new product to the database.
        /// </summary>
        /// <param name="file">The IFormFile representing the product image.</param>
        /// <param name="product">The Product object to add.</param>
        /// <returns>The added Product object.</returns>
        public async Task<Product> AddNewProduct(IFormFile file, Product product)
        {
            _context.Add(product);
            await _context.SaveChangesAsync();
            return product;
        }

        /// <summary>
        /// Deletes a product from the database by its ID.
        /// </summary>
        /// <param name="Id">The ID of the product to delete.</param>
        public async Task DeleteProduct(int Id)
        {
            var product = await _context.Products.FindAsync(Id);
            if (product == null)
                throw new KeyNotFoundException($"Product with id {Id} not found.");
            _context.Products.Remove(product);
            await _context.SaveChangesAsync();
        }

        /// <summary>
        /// Retrieves a list of all products in the database.
        /// </summary>
        /// <returns>A list of Product objects.</returns>
        public async Task<List<Product>> GetAllProducts()
        {
            var products = await _context.Products.ToListAsync();
            return products;
        }

        /// <summary>
        /// Retrieves a product by its ID.
        /// </summary>
        /// <param name="productID">The ID of the product to retrieve.</param>
        /// <returns>The retrieved Product object or null if not found.</returns>
        public async Task<Product> GetProductById(int productID)
        {
            var product = await _context.Products.FirstOrDefaultAsync(c => c.ProductId == productID);
            return product;
        }

        /// <summary>
        /// Retrieves a list of products by category.
        /// </summary>
        /// <param name="categoryID">The ID of the category.</param>
        /// <returns>A list of Product objects in the specified category.</returns>
        public async Task<List<Product>> GetProductsByCategory(int categoryID)
        {
            var products = await _context.Products
                .Where(id => id.CategoryId == categoryID)
                .ToListAsync();
            return products;
        }

        /// <summary>
        /// Updates a product in the database.
        /// </summary>
        /// <param name="Id">The ID of the product to update.</param>
        /// <param name="product">The updated Product object.</param>
        /// <returns>The updated Product object.</returns>
        public async Task<Product> UpdateProduct(int Id, Product product)
        {
            var existingProduct = await _context.Products.FindAsync(Id);
            existingProduct.ProductId = product.ProductId;
            existingProduct.Name = product.Name;
            existingProduct.Description = product.Description;
            existingProduct.Price = product.Price;
            existingProduct.StockQuantity = product.StockQuantity;
            existingProduct.CategoryId = product.CategoryId;
            if (product.ProductImage != null)
                existingProduct.ProductImage = product.ProductImage;

            _context.Update(existingProduct);
            await _context.SaveChangesAsync();
            return product;
        }

        /// <summary>
        /// Searches for products by a query string.
        /// </summary>
        /// <param name="query">The search query string.</param>
        /// <returns>A list of Product objects matching the search query.</returns>
        public async Task<List<Product>> Search(string query)
        {
            return await _context.Products
                .Where(p => p.Name.Contains(query))
                .ToListAsync();
        }
    }
}
