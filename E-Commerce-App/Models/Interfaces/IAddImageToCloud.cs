namespace E_Commerce_App.Models.Interfaces
{
    public interface IAddImageToCloud
    {
        Task<Product> UploadProductImage(IFormFile file, Product product);
        Task<Category> UploadCategoryImage(IFormFile file, Category category);
    }
}
