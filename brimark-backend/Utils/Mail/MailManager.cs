using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Threading.Tasks;

// Essentially account creation should update the database at the same time
// This should essentially process the database and basically turn on and off when something new is posted to prevent constant queries

namespace brimark_backend.Utils.Mail
{
    public static class MailManager
    {

        private static MySqlConnection connection;

        private static List<Mail> queue = new List<Mail>();
        private static MailWorker worker;
        private static SmtpClient client;

        static MailManager()
        {

            Dictionary<string, string> email = Data.GetEmail();
            client = new SmtpClient("smtp.gmail.com")
            {
                Port = 587,
                Credentials = new NetworkCredential(email["email"], email["password"]),
                EnableSsl = true,
            };

        }

        
        public static void Poke()
        {

            // If the daemon is dead, assume the database has been upstarted and restart the daemon
            if (!worker.IsAlive)
            {
                worker = new MailWorker(client, connection);
                worker.RunThread();
            }

        }

        public static void SetConnection(MySqlConnection connection)
        {
            MailManager.connection = connection;
            worker = new MailWorker(client, connection);
            worker.RunThread();
        }
    }
}
