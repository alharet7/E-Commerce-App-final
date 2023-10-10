namespace E_Commerce_App.Models.Interfaces
{
    public interface ICategory
    {
        Task<Category> CreateNewCategory(Category categoryDTO);
        Task<Category> GetCategoryById(int categoryID);
        Task<List<Category>> GetAllCategories();
        Task<Category> UpdateCategory(int Id, Category categoryDTO);
        Task DeleteCategory(int Id);
    }
}
