using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Threading;
using System.Threading.Tasks;

/*
 * 
 * Mail Client
 * Should fundamentally function as a separate server entity, and the api essentially passes requests to this which runs as a daemon
 * Queue mail requests in memory is okay but if mail cannot be sent for any reason and power is lost created accounts may never recieve activation emails,
 * hence we could potentially store email todos in database
 * This is completely unnecessary for our specific small scale use case but in the real world this could be useful
 * 
 * 
 */


// Needs some work
namespace brimark_backend.Utils
{
    public static class Mailer
    {

        private static readonly List<Mail> queue = new List<Mail>();
        private static readonly Task mailThread;

        static Mailer()
        {

            // Synchronize Queue With Database
            // Spawn Task Instance


        }

        public static void QueueMail(String userEmail, String hash)
        {
            queue.Add(new Mail(userEmail, hash));
        }

        private static Task mailerTask()
        {
            return new Task(() =>
            {
                while (true)
                {
                    foreach (Mail mailToSend in queue)
                    {



                    }
                }
            });
        }

        private static void SendMail(Mail mailToSend)
        {

        }

    }
}
