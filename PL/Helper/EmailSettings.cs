using Demo.DAL.Entities;
using System.Net;
using System.Net.Mail;

namespace PL.Helper
{
    public  class EmailSettings
    {
        public static void SendEmail(Email email)
        {
            var client = new SmtpClient("smtp.ethereal.email", 587);
            client.EnableSsl = true;

            client.Credentials = new NetworkCredential("rosa.larkin@ethereal.email", "7w6HtMdRQFVR9RQnuu");

            client.Send("rosa.larkin@ethereal.email", email.To, email.Title, email.Body);
        }
    }
}
