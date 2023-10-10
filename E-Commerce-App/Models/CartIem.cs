using System.ComponentModel.DataAnnotations.Schema;

namespace E_Commerce_App.Models
{
    public class CartItem
    {
        public int ID { get; set; }
        public int Quantity { get; set; }

        [ForeignKey("Order")]
        public int OrderID { get; set; }
        public Order Order { get; set; }
        public Product Product { get; set; }
    }
}
