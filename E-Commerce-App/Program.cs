using E_Commerce_App.Data;
using E_Commerce_App.Models;
using E_Commerce_App.Models.Interfaces;
using E_Commerce_App.Models.Services;
using E_Commerce_App.Services;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace E_Commerce_App
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.
            builder.Services.AddControllersWithViews().AddRazorRuntimeCompilation()
                .AddViewOptions(options =>
                {
                    options.HtmlHelperOptions.ClientValidationEnabled = true;
                    options.HtmlHelperOptions.Html5DateRenderingMode = Microsoft.AspNetCore.Mvc.Rendering.Html5DateRenderingMode.Rfc3339;
                })
                .AddDataAnnotationsLocalization();

            builder.Services.AddScoped<CartSidebarViewComponent>();
            builder.Services.AddScoped<IEmail, EmailService>();
            builder.Services.AddTransient<ICategory, CategoryServices>();
            builder.Services.AddTransient<IProduct, ProductServices>();
            builder.Services.AddTransient<IAddImageToCloud, AddImageService>();
            builder.Services.AddScoped<JWTTokenService>();
            builder.Services.AddTransient<IOrder, OrderService>();
            builder.Services.AddTransient<IPaymentService, PaymentService>();

            // builder.Services.AddControllers();
            string connString = builder.Configuration.GetConnectionString("DefaultConnection");
            builder.Services
                .AddDbContext<StoreDbContext>
                (opions => opions.UseSqlServer(connString));

            builder.Services.AddIdentity<ApplicationUser, IdentityRole>(options =>
            {
                options.User.RequireUniqueEmail = true;
            }).AddEntityFrameworkStores<StoreDbContext>();

            builder.Services.AddTransient<IUserService, IdentityUserService>();


            builder.Services.ConfigureApplicationCookie(options =>
            {
                options.ExpireTimeSpan = TimeSpan.FromDays(30);
                options.LoginPath = "/Auth/Index";

            });

            builder.Services.AddAuthentication();
            builder.Services.AddAuthorization();

            builder.Services.AddRazorPages();

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (!app.Environment.IsDevelopment())
            {
                app.UseExceptionHandler("/Home/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }


            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();

            app.UseAuthentication();
            app.UseAuthorization();

            app.MapRazorPages();

            app.MapControllerRoute(
                name: "default",
                pattern: "{controller=Home}/{action=Index}/{id?}");

            app.Run();
        }
    }
}