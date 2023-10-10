namespace E_Commerce_App.Models
{
    public class Category
    {
        public int CategoryId { get; set; }
        public string Name { get; set; }
        public string? imgURL { get; set; }
        // Navigation property 
        public IEnumerable<Product>? Products { get; set; }
    }
}
