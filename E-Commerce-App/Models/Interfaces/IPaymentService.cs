using Stripe.Checkout;

namespace E_Commerce_App.Models.Services
{
    public interface IPaymentService
    {
        Task<Session> PaymentProcess(Order order);
    }
}