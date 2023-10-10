using E_Commerce_App.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace E_Commerce_App.Data
{
    public class StoreDbContext : IdentityDbContext<ApplicationUser>
    {
        public StoreDbContext(DbContextOptions options) : base(options)
        {

        }
        public DbSet<Category> Categories { get; set; }
        public DbSet<Product> Products { get; set; }
        public DbSet<Order> Orders { get; set; }
        public DbSet<CartItem> CartItems { get; set; }


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // This calls the base method, but does nothing
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Category>().HasData(

                new Category
                {
                    CategoryId = 1,
                    Name = "Laptops",
                    imgURL = "https://lab29ecommerceimages.blob.core.windows.net/categoriesimages/cat laptops.png"
                },
                 new Category
                 {
                     CategoryId = 2,
                     Name = "Accessories",
                     imgURL = "https://lab29ecommerceimages.blob.core.windows.net/categoriesimages/istockphoto-1267943701-170667a.webp"
                 },
                 new Category
                 {
                     CategoryId = 3,
                     Name = "Screens",
                     imgURL = "https://lab29ecommerceimages.blob.core.windows.net/categoriesimages/cat screen.jpg"
                 }
                ); ;
            modelBuilder.Entity<Product>().HasData(
                new List<Product>
                    {
                    new Product
                     {
                         ProductId = 1,
                         Name = "MacBook Pro",
                         Description = "Powerful laptop for professionals",
                         Price = 1499,
                         StockQuantity = 50,
                         CategoryId = 1,
                         ProductImage = "https://lab29ecommerceimages.blob.core.windows.net/productsimages/mbp-spacegray-select-202206.jpeg"
                     },
                     new Product
                     {
                         ProductId = 2,
                         Name = "Dell XPS 13",
                         Description = "Sleek and high-performance laptop",
                         Price = 1299,
                         StockQuantity = 40,
                         CategoryId = 1,
                         ProductImage = "https://lab29ecommerceimages.blob.core.windows.net/productsimages/dell-xps-14-silver-1.jpg"
                     },
                     new Product
                     {
                         ProductId = 3,
                         Name = "Lenovo LEGION 5",
                         Description = "Gaming laptop",
                         Price = 999,
                         StockQuantity = 35,
                         CategoryId = 1,
                         ProductImage = "https://lab29ecommerceimages.blob.core.windows.net/productsimages/LEGion-5.jpeg"
                     },
                           new Product
                             {
                                 ProductId = 4,
                                 Name = "Laptop Bag",
                                 Description = "Stylish and durable laptop carrying bag",
                                 Price = 49,
                                 StockQuantity = 200,
                                 CategoryId = 2,
                                 ProductImage = "https://lab29ecommerceimages.blob.core.windows.net/productsimages/Lenovo-Toploader-T210-15.6-Inch-Casual-Laptop-Bag-Grey-.jpg"
                             },
                             new Product
                             {
                                 ProductId = 5,
                                 Name = "Wireless Mouse",
                                 Description = "Ergonomic wireless mouse",
                                 Price = 19,
                                 StockQuantity = 150,
                                 CategoryId = 2,
                                 ProductImage = "https://lab29ecommerceimages.blob.core.windows.net/productsimages/6333841_sd.jpg"
                             },
                             new Product
                             {
                                 ProductId = 6,
                                 Name = "Laptop Stand",
                                 Description = "Adjustable laptop stand for better ergonomics",
                                 Price = 29,
                                 StockQuantity = 100,
                                 CategoryId = 2,
                                 ProductImage = "https://lab29ecommerceimages.blob.core.windows.net/productsimages/Ergonomic-Adjustable-Foldable-Laptop-Stand-_2.jpg"
                             },
                        new Product
                        {
                            ProductId = 7,
                            Name = "24-Inch Monitor",
                            Description = "Full HD monitor for crisp visuals",
                            Price = 199,
                            StockQuantity = 30,
                            CategoryId = 3,
                            ProductImage = "https://lab29ecommerceimages.blob.core.windows.net/productsimages/24inchmonitors-2048px-9964.jpg"
                        },
                        new Product
                        {
                            ProductId = 8,
                            Name = "27-Inch 4K Monitor",
                            Description = "High-resolution 4K monitor with vibrant colors",
                            Price = 399,
                            StockQuantity = 20,
                            CategoryId = 3,
                            ProductImage = "https://lab29ecommerceimages.blob.core.windows.net/productsimages/4kmonitors-2048px-9782.jpg"
                        },
                        new Product
                        {
                            ProductId = 9,
                            Name = "Dual Monitor Stand",
                            Description = "Sturdy stand to hold two monitors for multitasking",
                            Price = 89,
                            StockQuantity = 10,
                            CategoryId = 3,
                            ProductImage = "https://lab29ecommerceimages.blob.core.windows.net/productsimages/vari-2-monitor_48003_silver_iso.jpg"
                        }
                        });

            var hasher = new PasswordHasher<ApplicationUser>();
            var Admin = new ApplicationUser
            {
                Id = "1",
                UserName = "Admin",
                NormalizedUserName = "ADMIN",
                Email = "adminUser@example.com",
                PhoneNumber = "1234567890",
                NormalizedEmail = "adminUser@EXAMPLE.COM",
                EmailConfirmed = true,
                LockoutEnabled = false
            };
            Admin.PasswordHash = hasher.HashPassword(Admin, "Admin@1+");

            modelBuilder.Entity<ApplicationUser>().HasData(Admin);

            var adminRoleId = "Administrator";
            modelBuilder.Entity<IdentityUserRole<string>>().HasData(new IdentityUserRole<string>
            {
                UserId = Admin.Id,
                RoleId = adminRoleId
            });


            SeedRole(modelBuilder, "Administrator");
            SeedRole(modelBuilder, "Editor");
            SeedRole(modelBuilder, "Users");


        }


        private void SeedRole(ModelBuilder modelBuilder, string roleName)
        {
            var role = new IdentityRole
            {
                Id = roleName.ToLower(),
                Name = roleName,
                NormalizedName = roleName.ToUpper(),
                ConcurrencyStamp = Guid.Empty.ToString()
            };
            modelBuilder.Entity<IdentityRole>().HasData(role);
        }
    }
}
