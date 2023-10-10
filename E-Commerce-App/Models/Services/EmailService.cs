using E_Commerce_App.Models.Interfaces;
using System.Net;
using System.Net.Mail;

namespace E_Commerce_App.Models.Services
{
    public class EmailService : IEmail
    {
        private readonly string apiKey = "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"; // replace with your actual API key

        public async Task SendEmail(string recieverEmail, string recieverName, string emailSubject, string emailBody)
        {
            var fromAddress = new MailAddress("MjhemPatata@gmail.com", "Tech Pioneers");
            var toAddress = new MailAddress(recieverEmail, recieverEmail.Substring(0, recieverEmail.IndexOf("@")));
            const string fromPassword = "4OhJ0cD3HY5vgdCq";
            string subject = emailSubject;
            string body = emailBody;

            var authAddress = new MailAddress("ahmadsa28121999@gmail.com", "ahmad saleh");

            var smtp = new SmtpClient
            {
                Host = "smtp-relay.brevo.com",
                Port = 587,
                EnableSsl = true,
                DeliveryMethod = SmtpDeliveryMethod.Network,
                UseDefaultCredentials = false,
                Credentials = new NetworkCredential(authAddress.Address, fromPassword)
            };

            using (var message = new MailMessage(fromAddress, toAddress)
            {
                Subject = subject,
                Body = body,
                IsBodyHtml = true
            })
            {
                message.Headers.Add("ApiKey", apiKey); // add the API key to the email headers

                await smtp.SendMailAsync(message);
            }
        }
    }
}
