using E_Commerce_App.Models;
using E_Commerce_App.Models.Interfaces;
using E_Commerce_App.Models.Services;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Newtonsoft.Json;
using System.Security.Claims;

namespace E_Commerce_App.Pages.OrderConfirmation
{
    public class OrderConfirmationModel : PageModel
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IOrder _order;
        private readonly IProduct _product;
        private readonly IEmail _email;
        private readonly IPaymentService _paymentService;

        public OrderConfirmationModel(UserManager<ApplicationUser> user, IOrder order, IProduct product, IEmail email, IPaymentService payment)
        {
            _userManager = user;
            _order = order;
            _product = product;
            _email = email;
            _paymentService = payment;
        }
        [BindProperty]
        public Order OrderConfirmed { get; set; }

        public List<CartItem> formCartItems { get; set; }

        public async Task OnGetAsync()
        {
            var productIdsCookie = HttpContext.Request.Cookies["productIds"];

            Dictionary<int, int> productQuantities = new Dictionary<int, int>();

            if (productIdsCookie != null)
            {
                productQuantities = JsonConvert.DeserializeObject<Dictionary<int, int>>(productIdsCookie);
            }

            formCartItems = new List<CartItem>();
            decimal? totalPrice = 0;

            foreach (var item in productQuantities)
            {
                var product = await _product.GetProductById(item.Key);
                var cartItem = new CartItem
                {
                    Product = product,
                    Quantity = item.Value,
                };
                totalPrice += product.Price * item.Value;
                formCartItems.Add(cartItem);
            }

            var userID = User.FindFirstValue(ClaimTypes.NameIdentifier);

            OrderConfirmed = new Order()
            {
                OrderDate = DateTime.Now,
                UserId = userID,
                TotalPrice = totalPrice,
                CartItems = formCartItems
            };

            await _order.Create(OrderConfirmed);

        }

        public async Task<IActionResult> OnPost(Order order)
        {
            order = await _order.Update(order.ID, order);

            var userName = User.FindFirstValue(ClaimTypes.Name);

            var userEmail = User.FindFirstValue(ClaimTypes.Email);

            string emailSubject = "Order Summary";

            string cartItemsBody = "";
            foreach (var item in order.CartItems)
            {
                cartItemsBody += $"<p><strong>{item.Product.Name}</strong>: ${item.Product.Price} &times; {item.Quantity}</p>";
            }

            var dynamicBodyText = $@"
    <html>
    <head>
        <style>
            body {{
                font-family: Arial, sans-serif;
                margin: 0;
                padding: 0;
                background-color: #f5f5f5;
            }}
            .container {{
                max-width: 600px;
                margin: 0 auto;
                padding: 20px;
                background-color: #ffffff;
                border-radius: 10px;
                box-shadow: 0 2px 4px rgba(0, 0, 0, 0.1);
            }}
            h3 {{
                color: #333;
            }}
            hr {{
                border: 1px solid #ddd;
                margin: 20px 0;
            }}
            p {{
                color: #555;
            }}
        </style>
    </head>
    <body>
        <div class='container'>
            <h3>Thank you for your order from Tech Pioneers</h3>
            <hr />
            <p><strong>Order ID:</strong> {order.ID}</p>
            {cartItemsBody}
            <p><strong>Total:</strong> ${order.TotalPrice}</p>
            <p>We thank you for your order and hope you visit us again soon.</p>
        </div>
    </body>
    </html>";

            await _email.SendEmail(userEmail, userName, emailSubject, dynamicBodyText);

            var session = await _paymentService.PaymentProcess(order);

            Response.Headers.Add("Location", session.Url);

            //if (Request.Cookies["productIds"] != null)
            //{
            //    Response.Cookies.Delete("productIds");
            //}

            return new StatusCodeResult(303);
        }

    }
}
