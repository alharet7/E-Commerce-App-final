namespace E_Commerce_App.Models.Interfaces
{
    public interface IOrder
    {
        public Task<Order> Create(Order order);

        public Task<List<Order>> GetAll();

        public Task<Order> GetByID(int orderID);
        public Task<Order> GetByUserID(string orderID);

        public Task<Order> Update(int orderID, Order NewOrder);

        public Task<Order> GetOrderSummary(Order order);
    }
}
