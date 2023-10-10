using E_Commerce_App.Data;
using E_Commerce_App.Models.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace E_Commerce_App.Models.Services
{
    public class OrderService : IOrder
    {
        private readonly StoreDbContext _context;
        private readonly IEmail _email;

        public OrderService(StoreDbContext context, IEmail email)
        {
            _context = context;
            _email = email;

        }


        public async Task<Order> Create(Order order)
        {
            _context.Add(order);
            await _context.SaveChangesAsync();
            order.ID = order.ID;
            return order;
        }

        public async Task<List<Order>> GetAll()
        {
            var orders = await _context.Orders
                .Include(o => o.CartItems)
                .ThenInclude(p => p.Product)
                .ToListAsync();

            if (orders == null)
            {
                throw new KeyNotFoundException($"No orders found.");
            }

            foreach (var order in orders)
            {
                var user = await _context.Users.FirstOrDefaultAsync(u => u.Id == order.UserId);
                if (user != null)
                {
                    order.UserId = user.UserName;
                }
            }

            return orders;
        }


        public async Task<Order> GetByID(int orderID)
        {
            var order = await _context.Orders
                .Include(o => o.CartItems)
                .ThenInclude(p => p.Product)
                .FirstOrDefaultAsync(id => id.ID == orderID);

            if (order == null)
            {
                throw new KeyNotFoundException($"Category with id {orderID} not found.");
            }
            return order;
        }

        public async Task<Order> GetByUserID(string orderID)
        {
            var order = await _context.Orders
                .Include(o => o.CartItems)
                .ThenInclude(p => p.Product)
                .OrderBy(oDate => oDate.OrderDate)
                .LastOrDefaultAsync(oID => oID.UserId == orderID);

            if (order == null)
            {
                throw new KeyNotFoundException($"Category with id {orderID} not found.");
            }
            return order;
        }

        public async Task<Order> Update(int orderID, Order NewOrder)
        {
            var oldorder = await GetByID(orderID);

            oldorder.ID = orderID;
            oldorder.OrderDate = NewOrder.OrderDate;
            oldorder.PhoneNumber = NewOrder.PhoneNumber;
            oldorder.StreetAddress = NewOrder.StreetAddress;
            oldorder.City = NewOrder.City;
            oldorder.UserId = NewOrder.UserId;
            oldorder.PostalCode = NewOrder.PostalCode;
            oldorder.TotalPrice = NewOrder.TotalPrice;
            oldorder.CartItems = oldorder.CartItems;

            _context.Orders.Update(oldorder);
            await _context.SaveChangesAsync();
            return oldorder;

        }

        public async Task<Order> GetOrderSummary(Order order)
        {
            return order;
        }
    }
}
