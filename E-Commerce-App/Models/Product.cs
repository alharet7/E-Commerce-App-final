using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace E_Commerce_App.Models
{
    public class Product
    {
        public int ProductId { get; set; }

        // Foreign key
        [Display(Name = "Category")]
        public int CategoryId { get; set; }

        [Display(Name = "Product Name")]
        public string Name { get; set; }
        public string Description { get; set; }

        [Column(TypeName = "money")]
        public decimal Price { get; set; }
        public int StockQuantity { get; set; }
        public string? ProductImage { get; set; }

        //public byte[] ProductImage { get; set; }

        // Navigation property 
        public Category? Category { get; set; }
    }
}
