namespace E_Commerce_App.Models.ViewModels
{
    public class OrderConfirmationVM
    {
        public List<CartItem> CartItems { get; set; }

        public Order Order { get; set; }
    }
}
