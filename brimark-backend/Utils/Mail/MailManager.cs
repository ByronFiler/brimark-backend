using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

// Essentially account creation should update the database at the same time
// This should essentially process the database and basically turn on and off when something new is posted to prevent constant queries

namespace brimark_backend.Utils.Mail
{
    public static class MailManager
    {

        private static List<Mail> queue = new List<Mail>();
        private static MailWorker worker;

        static MailManager()
        {
            
            // Synchronize with Database to Build Queue, LOG

        }

        public static void SendMail(String userEmail, String hash)
        {

            // Update the Database
            
        }


        // Method to Add To Queue
        // Manage Queue
        // Control thread locking of mailer thread
        // Handle ALL database requests
        // Handle error logging

    }
}
