using E_Commerce_App.Models;
using E_Commerce_App.Models.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

public class CartSidebarViewComponent : ViewComponent
{
    private readonly IProduct _product;

    public CartSidebarViewComponent(IProduct product)
    {
        _product = product;
    }

    public async Task<IViewComponentResult> InvokeAsync()
    {
        var productIdsCookie = HttpContext.Request.Cookies["productIds"];
        Dictionary<int, int> productQuantities = new Dictionary<int, int>();

        if (productIdsCookie != null)
        {
            try
            {
                // Try to deserialize as dictionary
                productQuantities = JsonConvert.DeserializeObject<Dictionary<int, int>>(productIdsCookie);
            }
            catch (JsonSerializationException)
            {
                // If deserialization as dictionary fails, try as list
                var productIds = JsonConvert.DeserializeObject<List<int>>(productIdsCookie);
                foreach (var id in productIds)
                {
                    if (!productQuantities.ContainsKey(id))
                    {
                        productQuantities[id] = 1;
                    }
                    else
                    {
                        productQuantities[id]++;
                    }
                }
            }
        }

        var cartItems = new List<CartItem>();
        foreach (var item in productQuantities)
        {
            var product = await _product.GetProductById(item.Key);
            var cartItem = new CartItem
            {
                Product = product,
                Quantity = item.Value
            };
            cartItems.Add(cartItem);
        }

        return View(cartItems);
    }
}
