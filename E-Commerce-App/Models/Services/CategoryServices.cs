using E_Commerce_App.Data;
using E_Commerce_App.Models;
using E_Commerce_App.Models.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace E_Commerce_App.Services
{
    /// <summary>
    /// Service for managing categories in the database.
    /// </summary>
    public class CategoryServices : ICategory
    {
        private readonly StoreDbContext _context;

        /// <summary>
        /// Initializes a new instance of the CategoryServices class.
        /// </summary>
        /// <param name="context">The StoreDbContext used for database operations.</param>
        public CategoryServices(StoreDbContext context)
        {
            _context = context;
        }

        /// <summary>
        /// Creates a new category in the database.
        /// </summary>
        /// <param name="category">The category to be created.</param>
        /// <returns>The created category.</returns>
        public async Task<Category> CreateNewCategory(Category category)
        {
            _context.Categories.Add(category);

            await _context.SaveChangesAsync();

            category.CategoryId = category.CategoryId;

            return category;
        }

        /// <summary>
        /// Deletes a category from the database by its ID.
        /// </summary>
        /// <param name="Id">The ID of the category to delete.</param>
        public async Task DeleteCategory(int Id)
        {
            var category = await _context.Categories.FindAsync(Id);

            if (category != null)
            {
                _context.Categories.Remove(category);
                await _context.SaveChangesAsync();
            }
        }

        /// <summary>
        /// Retrieves a list of all categories from the database.
        /// </summary>
        /// <returns>A list of all categories.</returns>
        public async Task<List<Category>> GetAllCategories()
        {
            var categories = await _context.Categories.ToListAsync();

            return categories;
        }

        /// <summary>
        /// Retrieves a category from the database by its ID.
        /// </summary>
        /// <param name="categoryID">The ID of the category to retrieve.</param>
        /// <returns>The retrieved category or null if not found.</returns>
        public async Task<Category> GetCategoryById(int categoryID)
        {
            var category = await _context.Categories.FindAsync(categoryID);
            if (category == null)
            {
                throw new KeyNotFoundException($"Category with id {categoryID} not found.");
            }
            return category;
        }

        /// <summary>
        /// Updates a category in the database by its ID.
        /// </summary>
        /// <param name="Id">The ID of the category to update.</param>
        /// <param name="categoryDTO">The updated category data.</param>
        /// <returns>The updated category or null if not found.</returns>
        public async Task<Category> UpdateCategory(int Id, Category categoryDTO)
        {
            var category = await _context.Categories.FindAsync(Id);

            if (category == null)
            {
                throw new KeyNotFoundException($"Category with id {Id} not found.");
            }

            category.Name = categoryDTO.Name;
            if (categoryDTO.imgURL != null)
                category.imgURL = categoryDTO.imgURL;

            _context.Categories.Update(category);

            await _context.SaveChangesAsync();

            return categoryDTO;
        }
    }
}
