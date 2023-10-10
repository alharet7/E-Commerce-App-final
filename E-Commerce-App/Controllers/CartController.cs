using E_Commerce_App.Models.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Security.Claims;

namespace E_Commerce_App.Controllers
{
    public class CartController : Controller
    {
        private readonly IProduct _product;
        private readonly IOrder _order;

        public CartController(IProduct product, IOrder order)
        {
            _product = product;
            _order = order;
        }

        [HttpGet]
        public async Task<IActionResult> Index()
        {
            return NoContent();
        }

        public async Task<IActionResult> OrderSummary()
        {
            var productIdsCookie = HttpContext.Request.Cookies["productIds"];
            if (productIdsCookie != null)
            {
                var userID = User.FindFirstValue(ClaimTypes.NameIdentifier);

                var order = await _order.GetByUserID(userID);

                if (productIdsCookie != null)
                {
                    Response.Cookies.Delete("productIds");
                }

                return View(order);
            }

            else

                return NoContent();
        }

        [HttpPost]
        public IActionResult AddToCart(int productId)
        {
            var productIdsCookie = HttpContext.Request.Cookies["productIds"];
            Dictionary<int, int> productQuantities;
            if (productIdsCookie != null)
            {
                productQuantities = JsonConvert.DeserializeObject<Dictionary<int, int>>(productIdsCookie);
                if (productQuantities.ContainsKey(productId))
                {
                    productQuantities[productId] += 1;
                }
                else
                {
                    productQuantities[productId] = 1;
                }
            }
            else
            {
                productQuantities = new Dictionary<int, int>
                 {
                         { productId, 1 }
                 };
            }

            var options = new CookieOptions
            {
                Expires = DateTime.Now.AddDays(30),
                IsEssential = true
            };

            HttpContext.Response.Cookies.Append("productIds", JsonConvert.SerializeObject(productQuantities), options);
            string referrerUrl = Request.Headers["Referer"].ToString();
            return Redirect(referrerUrl);
        }

        public IActionResult RemoveFromCart(int productId)
        {
            var productIdsCookie = HttpContext.Request.Cookies["productIds"];
            Dictionary<int, int> productQuantities;
            if (productIdsCookie != null)
            {
                productQuantities = JsonConvert.DeserializeObject<Dictionary<int, int>>(productIdsCookie);
                if (productQuantities.ContainsKey(productId))
                {
                    productQuantities[productId] -= 1;

                    if (productQuantities[productId] <= 0)
                    {
                        productQuantities.Remove(productId);
                    }

                    var options = new CookieOptions
                    {
                        Expires = DateTime.Now.AddDays(30),
                        IsEssential = true
                    };

                    HttpContext.Response.Cookies.Append("productIds", JsonConvert.SerializeObject(productQuantities), options);
                }
            }
            string referrerUrl = Request.Headers["Referer"].ToString();
            return Redirect(referrerUrl);
        }

        public async Task<IActionResult> UpdateCart(int productId, string changeQuantity)
        {
            var productIdsCookie = HttpContext.Request.Cookies["productIds"];
            Dictionary<int, int> productQuantities;
            if (productIdsCookie != null)
            {
                productQuantities = JsonConvert.DeserializeObject<Dictionary<int, int>>(productIdsCookie);
                if (productQuantities.ContainsKey(productId))
                {
                    if (changeQuantity == "-1")
                    {
                        productQuantities[productId] -= 1;
                    }
                    else if (changeQuantity == "+1")
                    {
                        var product = await _product.GetProductById(productId);

                        if (productQuantities[productId] < product.StockQuantity)
                        {
                            productQuantities[productId] += 1;
                        }
                        else
                        {
                            TempData["warning"] = "Cannot add more items. You've reached the maximum stock available.";
                        }
                    }
                    else if (changeQuantity == "update")
                    {
                        int newQuantity;
                        if (int.TryParse(HttpContext.Request.Form["quantity"], out newQuantity))
                        {
                            var product = await _product.GetProductById(productId);

                            if (newQuantity > product.StockQuantity)
                            {

                                newQuantity = product.StockQuantity;
                                TempData["warning"] = "Requested quantity is greater than stock. Quantity has been set to maximum available.";
                            }

                            productQuantities[productId] = newQuantity;
                        }
                    }

                    else
                    {
                        int newQuantity;
                        if (int.TryParse(changeQuantity, out newQuantity))
                        {
                            productQuantities[productId] = newQuantity;
                        }
                    }

                    if (productQuantities[productId] <= 0)
                    {
                        productQuantities.Remove(productId);
                    }

                    var options = new CookieOptions
                    {
                        Expires = DateTime.Now.AddDays(30),
                        IsEssential = true
                    };

                    HttpContext.Response.Cookies.Append("productIds", JsonConvert.SerializeObject(productQuantities), options);
                }
            }
            string referrerUrl = Request.Headers["Referer"].ToString();
            return Redirect(referrerUrl);
        }
    }
}
