using System.ComponentModel.DataAnnotations;

namespace E_Commerce_App.Models.DTOs
{
    public class OrderDTO
    {
        public string UserId { get; set; }
        public string Address { get; set; }

        [Display(Name = "Order Date")]
        public DateTime? OrderDate { get; set; }

        [Display(Name = "Total Price")]
        public decimal TotalPrice { get; set; }
    }
}
