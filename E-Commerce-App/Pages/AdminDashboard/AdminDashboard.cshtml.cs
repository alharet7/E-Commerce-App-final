using E_Commerce_App.Data;
using E_Commerce_App.Models;
using E_Commerce_App.Models.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;

namespace E_Commerce_App.Pages.AdminDashboard
{
    public class AdminDashboardModel : PageModel
    {
        private readonly IOrder _order;
        private readonly StoreDbContext _storeDBContext;

        public List<Order> Orders { get; set; }
        public AdminDashboardModel(IOrder order, StoreDbContext dbContext)
        {
            _order = order;
            _storeDBContext = dbContext;
        }
        public async Task<IActionResult> OnGet()
        {
            Orders = await _order.GetAll();

            return Page();
        }

        public async Task<string> GetUserName(string userId)
        {
            var user = await _storeDBContext.Users.FirstOrDefaultAsync(u => u.Id == userId);
            return user?.UserName;
        }


    }
}
