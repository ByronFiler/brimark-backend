using System;
using System.Net;
using System.Net.Mail;

/*
 * https://blog.elmah.io/how-to-send-emails-from-csharp-net-the-definitive-tutorial/
 * Alternative configuration available where we can store the settings in app cofig instead which is preferable
 */

namespace brimark_backend.Utils.Mail
{
    public class MailWorker : BaseThread
    {

        private readonly SmtpClient client;
        private readonly string brimarkEmail = "";
        private readonly string brimarkPassword = "";

        public MailWorker() : base()
        {

            this.client = new SmtpClient("smtp.gmail.com")
            {
                Port = 587,
                Credentials = new NetworkCredential(brimarkEmail, brimarkPassword),
                EnableSsl = true,
            };

        }

        public override void RunThread()
        {

            // Read Unsent Mails from Database

            // Send Emails

        }

        private void Mail(Mail email)
        {

            try
            {
                MailMessage message = new MailMessage
                {
                    From = new MailAddress(brimarkEmail),
                    Subject = "",                           // Translation Required? JORDAN ON THIS
                    Body = "",                              // Translaation Required? JORDAN ON THIS
                    IsBodyHtml = true,
                };
                message.To.Add(email.UserEmail);

                client.Send(message);

                // Email Successfully Sent, Log
                // Update database

            } catch (SmtpException e)
            {
                // Failed to Send Email
                // Readd to Queue and Log This As Warning
                

            }
        

        }

    }
}
