using Azure.Storage.Blobs;
using Azure.Storage.Blobs.Models;
using E_Commerce_App.Models.Interfaces;

namespace E_Commerce_App.Models.Services
{
    /// <summary>
    /// Service for uploading images to Azure Blob Storage.
    /// </summary>
    public class AddImageService : IAddImageToCloud
    {
        private readonly IConfiguration _configuration;

        /// <summary>
        /// Initializes a new instance of the AddImageService class.
        /// </summary>
        /// <param name="config">The IConfiguration instance used for accessing Azure Blob Storage configuration.</param>
        public AddImageService(IConfiguration config)
        {
            _configuration = config;
        }

        /// <summary>
        /// Uploads a product image to Azure Blob Storage.
        /// </summary>
        /// <param name="file">The IFormFile containing the image data.</param>
        /// <param name="product">The Product object to which the image is associated.</param>
        /// <returns>The updated Product object with the image URL.</returns>
        public async Task<Product> UploadProductImage(IFormFile file, Product product)
        {
            BlobContainerClient blobContainerClient =
                new BlobContainerClient
                (_configuration.GetConnectionString("StorageAccount"), "productsimages");

            await blobContainerClient.CreateIfNotExistsAsync();

            BlobClient blobClient = blobContainerClient.GetBlobClient(file.FileName);

            using var filestream = file.OpenReadStream();

            BlobUploadOptions blobUploadOptions = new BlobUploadOptions
            {
                HttpHeaders = new BlobHttpHeaders { ContentType = file.ContentType }
            };

            if (!blobClient.Exists())
                await blobClient.UploadAsync(filestream, blobUploadOptions);

            product.ProductImage = blobClient.Uri.ToString();
            return product;
        }

        /// <summary>
        /// Uploads a category image to Azure Blob Storage.
        /// </summary>
        /// <param name="file">The IFormFile containing the image data.</param>
        /// <param name="category">The Category object to which the image is associated.</param>
        /// <returns>The updated Category object with the image URL.</returns>
        public async Task<Category> UploadCategoryImage(IFormFile file, Category category)
        {
            BlobContainerClient blobContainerClient =
                new BlobContainerClient
                (_configuration.GetConnectionString("StorageAccount"), "categoriesimages");

            await blobContainerClient.CreateIfNotExistsAsync();

            BlobClient blobClient = blobContainerClient.GetBlobClient(file.FileName);

            using var filestream = file.OpenReadStream();

            BlobUploadOptions blobUploadOptions = new BlobUploadOptions
            {
                HttpHeaders = new BlobHttpHeaders { ContentType = file.ContentType }
            };

            if (!blobClient.Exists())
                await blobClient.UploadAsync(filestream, blobUploadOptions);

            category.imgURL = blobClient.Uri.ToString();
            return category;
        }
    }
}
