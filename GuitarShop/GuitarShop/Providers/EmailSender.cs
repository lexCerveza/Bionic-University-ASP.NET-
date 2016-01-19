using System.Configuration;
using System.Net;
using System.Net.Mail;
using GuitarShop.Models;
using MailMessage = AE.Net.Mail.MailMessage;

namespace GuitarShop.Providers
{
    public class EmailSender : ISender
    {
        private const string UsernameKey = "username";
        private const string PasswordKey = "password";

        public bool SendInfoToClient(Purchase purchase)
        {
            var shopEmail = ConfigurationManager.AppSettings[UsernameKey];

            var messageToClient = new MailMessage
            {
                Subject = "Guitar Shop",
                Body = $"Dear {purchase.Name}. Your purchase id is {purchase.PurchaseId} - {purchase.GuitarName}.\n" +
                       "Wait for our call for a sec.",
                From = new MailAddress(shopEmail)
            };
            messageToClient.To.Add(new MailAddress(purchase.Email));

            var messageToShop = new MailMessage
            {
                Subject = "Client bought a new stuff",
                Body = $"Client {purchase.Name} with id - {purchase.PurchaseId} bought a {purchase.GuitarName}",
                From = new MailAddress(shopEmail)
            };
            messageToShop.To.Add(new MailAddress(shopEmail));

            using (var smtpServer = new SmtpClient("smtp.gmail.com")
            {
                Port = 587,
                EnableSsl = true,
                Credentials = new NetworkCredential(shopEmail, ConfigurationManager.AppSettings[PasswordKey]),
                Timeout = 5000
            })
            {
                try
                {
                    smtpServer.Send(messageToClient);
                    smtpServer.Send(messageToShop);
                }
                catch (SmtpException)
                {
                    smtpServer.Dispose();
                    return false;
                }
            }

            return true;
        }
    }
}
