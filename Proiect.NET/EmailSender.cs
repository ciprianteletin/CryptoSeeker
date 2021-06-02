using System.Net;
using System.Net.Mail;

namespace Proiect.NET
{
    class EmailSender
    {
        private static string email = "cryptocr.finder@gmail.com";
        private static string password = "cryptocrypto";

        public static void SendEmail(string emailToSendTo, string username)
        {
            MailMessage mail = new MailMessage();
            SmtpClient SmtpServer = new SmtpClient("smtp.gmail.com");
            mail.From = new MailAddress(email);
            mail.To.Add(emailToSendTo);
            mail.Subject = "Registration to CryptoFinder!";
            mail.Body = "Welcome dear" + username +  " to CryptoFinder!\n" +
                "We are pleased to see you on our side! Hope that you will enjoy our app and that it fits all your needs! \n" +
                "For any problem, contact CryptoFinder support team! Stay tuned!\n" +
                "\n" +
                "CryptoFinder team";

            SmtpServer.Port = 587;
            SmtpServer.Credentials = new NetworkCredential(email, password);
            SmtpServer.EnableSsl = true;

            SmtpServer.Send(mail);

        }
        private EmailSender()
        {
        }
    }
}
