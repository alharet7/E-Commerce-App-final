using E_Commerce_App.Data;
using E_Commerce_App.Models;
using E_Commerce_App.Models.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Xunit;

namespace E_Commerce_App_Tests
{
    public class ProductServiceTests : Mock
    {
        private readonly ProductServices _productService;

        public ProductServiceTests()
        {

            var serviceProvider = new ServiceCollection()
                .AddLogging()
                .BuildServiceProvider();

            var factory = serviceProvider.GetService<ILoggerFactory>();

            var logger = factory.CreateLogger<ProductServices>();

            _productService = new ProductServices(_db);
        }

        [Fact]
        public async Task AddNewProduct_ShouldCreateProduct()
        {
            // Arrange
            var product = new Product
            {
                Name = "Test Product",
                Price = 10.50m,
                Description = "Test Description",
                StockQuantity = 10,
                CategoryId = 1,
            };

            // Act
            var addedProduct = await _productService.AddNewProduct(null, product);

            // Assert
            Assert.NotNull(addedProduct);
            Assert.NotEqual(0, addedProduct.ProductId);
        }


        [Fact]
        public async Task DeleteProduct_ShouldDeleteProduct()
        {
            // Arrange
            var product = new Product
            {
                Name = "Test Product",
                Price = 10.50m,
                Description = "Test Description",
                StockQuantity = 10,
                CategoryId = 1,
            };
            _db.Products.Add(product);
            await _db.SaveChangesAsync();

            // Act
            await _productService.DeleteProduct(product.ProductId);

            // Assert
            var deletedProduct = await _db.Products.FindAsync(product.ProductId);
            Assert.Null(deletedProduct);
        }

        [Fact]
        public async Task GetProductById_ShouldReturnCorrectProduct()
        {
            // Arrange
            var product = new Product
            {
                Name = "Test Product",
                Price = 10.50m,
                Description = "Test Description",
                StockQuantity = 5,
                CategoryId = 1,

            };
            _db.Products.Add(product);
            await _db.SaveChangesAsync();

            // Act
            var result = await _productService.GetProductById(product.ProductId);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(product.Name, result.Name);
            Assert.Equal(product.Price, result.Price);
            Assert.Equal(product.Description, result.Description);
            Assert.Equal(product.StockQuantity, result.StockQuantity);
            Assert.Equal(product.CategoryId, result.CategoryId);

        }


        [Fact]
        public async Task GetProductsByCategory_ShouldReturnCorrectProducts()
        {
            // Arrange
            var category = new Category { Name = "TestCategory" };
            _db.Categories.Add(category);
            await _db.SaveChangesAsync();

            var products = new List<Product>
    {
        new Product
        {
            Name = "Product 1",
            Description = "Description 1",
            Price = 10.50m,
            StockQuantity = 10,
            ProductImage = "Image URL 1",
            CategoryId = category.CategoryId
        },

    };
            _db.Products.AddRange(products);
            await _db.SaveChangesAsync();

            // Act
            var result = await _productService.GetProductsByCategory(category.CategoryId);

            // Assert
            Assert.Equal(1, result.Count);
        }



        [Fact]
        public async Task UpdateProduct_ShouldUpdateProduct()
        {
            // Arrange
            var product = new Product
            {
                Name = "Test Product",
                Price = 10.50m,
                Description = "Test Description",
                StockQuantity = 100,
                CategoryId = 1,
                ProductImage = "test.jpg"
            };
            _db.Products.Add(product);
            await _db.SaveChangesAsync();


            _db.Entry(product).State = EntityState.Detached;

            var updatedProduct = new Product
            {
                ProductId = product.ProductId,
                Name = "Updated Product",
                Price = 15.75m,
                Description = "Updated Description",
                StockQuantity = 50,
                CategoryId = 2,
                ProductImage = "updated.jpg"
            };

            // Act
            var result = await _productService.UpdateProduct(product.ProductId, updatedProduct);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(updatedProduct.Name, result.Name);
            Assert.Equal(updatedProduct.Price, result.Price);
            Assert.Equal(updatedProduct.Description, result.Description);
            Assert.Equal(updatedProduct.StockQuantity, result.StockQuantity);
            Assert.Equal(updatedProduct.CategoryId, result.CategoryId);
            Assert.Equal(updatedProduct.ProductImage, result.ProductImage);
        }


    }
}
