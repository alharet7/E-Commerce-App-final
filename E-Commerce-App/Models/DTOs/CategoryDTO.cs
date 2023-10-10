namespace E_Commerce_App.Models.DTOs
{
    public class CategoryDTO
    {
        public int CategoryId { get; set; }
        public string Name { get; set; }

        // Navigation property 
        //  public IEnumerable<Product>? Products { get; set; }
    }
}
