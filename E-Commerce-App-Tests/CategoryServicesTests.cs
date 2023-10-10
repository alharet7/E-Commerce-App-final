using E_Commerce_App.Models;
using E_Commerce_App.Models.Interfaces;
using E_Commerce_App.Services;

namespace E_Commerce_App_Tests
{
    public class CategoryServicesTests : Mock
    {
        private readonly ICategory _categoryService;

        public CategoryServicesTests()
        {
            _categoryService = new CategoryServices(_db);
        }

        [Fact]
        public async Task CreateNewCategory_ShouldCreateCategory()
        {
            // Arrange
            var category = new Category { Name = "Test Category" };

            // Act
            var addedCategory = await _categoryService.CreateNewCategory(category);

            // Assert
            Assert.NotNull(addedCategory);
            Assert.NotEqual(0, addedCategory.CategoryId);
        }

        [Fact]
        public async Task DeleteCategory_ShouldDeleteCategory()
        {
            // Arrange
            var category = new Category { Name = "Test Category" };
            _db.Categories.Add(category);
            await _db.SaveChangesAsync();

            // Act
            await _categoryService.DeleteCategory(category.CategoryId);

            // Assert
            var deletedCategory = await _db.Categories.FindAsync(category.CategoryId);
            Assert.Null(deletedCategory);
        }



        [Fact]
        public async Task GetCategoryById_ShouldReturnCorrectCategory()
        {
            // Arrange
            var category = new Category { Name = "Test Category" };
            _db.Categories.Add(category);
            await _db.SaveChangesAsync();

            // Act
            var result = await _categoryService.GetCategoryById(category.CategoryId);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(category.Name, result.Name);
        }

        [Fact]
        public async Task UpdateCategory_ShouldUpdateCategory()
        {
            // Arrange
            var category = new Category { Name = "Test Category" };
            _db.Categories.Add(category);
            await _db.SaveChangesAsync();

            var updatedCategory = new Category
            {
                Name = "Updated Category",
                imgURL = "https://example.com/image.jpg"
            };

            // Act
            var result = await _categoryService.UpdateCategory(category.CategoryId, updatedCategory);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(updatedCategory.Name, result.Name);
            Assert.Equal(updatedCategory.imgURL, result.imgURL);
        }
    }
}
