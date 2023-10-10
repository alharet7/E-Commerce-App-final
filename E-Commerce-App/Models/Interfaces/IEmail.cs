namespace E_Commerce_App.Models.Interfaces
{
    public interface IEmail
    {
        public Task SendEmail(string recieverEmail, string recieverName, string emailSubject, string emailBody);
    }
}
