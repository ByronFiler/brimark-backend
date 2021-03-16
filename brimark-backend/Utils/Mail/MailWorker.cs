using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Diagnostics;
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

        private static readonly MySqlCommand getUnsentEmails = new MySqlCommand(@"SELECT * FROM `emails` WHERE sent=0;");
        private static readonly MySqlCommand getRelevantAccount = new MySqlCommand(@"SELECT email, name, activation_hash FROM `accounts` WHERE id=@accountId;");
        private static readonly MySqlCommand markEmailSent = new MySqlCommand(@"UPDATE `emails` SET sent=1,date_sent=@date WHERE accountId=@accountId;");

        private static readonly MySqlParameter accountIdParameter = new MySqlParameter("@accountId", MySqlDbType.Int64);
        private static readonly MySqlParameter dateParameter = new MySqlParameter("@date", MySqlDbType.Date);

        public MailWorker(SmtpClient client, MySqlConnection connection) : base()
        {

            getUnsentEmails.Connection = connection;
            getRelevantAccount.Connection = connection;
            markEmailSent.Connection = connection;

            this.client = client;
        }

        public override void RunThread()
        {

            while (true)
            {
                Debug.WriteLine("Running Mail Thread");

                List<string> accountsToEmail = new List<string>();
                List<string> successfullyEmailedAccounts = new List<string>();

                using (MySqlDataReader unsentEmails = getUnsentEmails.ExecuteReader())
                {
                    while (unsentEmails.Read())
                        accountsToEmail.Add(unsentEmails.GetString(1));
                }

                if (accountsToEmail.Count == 0)
                    break;

                foreach (string accountId in accountsToEmail)
                {
                    accountIdParameter.Value = accountId;

                    getRelevantAccount.Parameters.Add(accountIdParameter);
                    getRelevantAccount.Prepare();

                    using (MySqlDataReader accountReader = getRelevantAccount.ExecuteReader())
                    {
                        if (accountReader.Read())
                        {
                            if (Mail(accountReader.GetString(0), accountReader.GetString(1), accountReader.GetString(2)))
                            {

                                successfullyEmailedAccounts.Add(accountId);
                                Debug.WriteLine("Email successfully sent to: " + accountReader.GetString(1));

                            }
                            else
                            {

                                // Email not successfully sent :(
                                Debug.WriteLine("[WARNING] Failed to email: " + accountReader.GetString(1));


                            }
                        }
                        else
                        {
                            Debug.WriteLine("[FATAL] Database Error retrieving account data.");
                            System.Environment.Exit(1);

                        }
                    }

                }

                foreach (string accountId in successfullyEmailedAccounts)
                {
                    try
                    {
                        dateParameter.Value = DateTime.Now.ToString("yyyy-MM-dd");
                        accountIdParameter.Value = accountId;
                        
                        markEmailSent.Parameters.Add(dateParameter);
                        markEmailSent.Parameters.Add(accountIdParameter);

                        markEmailSent.Prepare();
                        markEmailSent.ExecuteNonQuery();
                    }
                    catch (Exception e)
                    {
                        Debug.WriteLine("[FATAL] Failed to update database.");
                        Debug.WriteLine("    -> " + e.Message);
                        Debug.WriteLine(e.StackTrace);
                        System.Environment.Exit(1);
                    }
                }
            }

            
            Debug.WriteLine("No Accounts To Email, Terminated Mail Thread");
        }

        
        private bool Mail(String recipient, String recipient_username, String activation_hash)
        {
            try
            {
                MailMessage message = new MailMessage
                {
                    From = new MailAddress(recipient),
                    Subject = "Brimark Account Activation",                           
                    Body = String.Format("Dear {0},<br><br>" +
                    "" +
                    "To activate your account please click the following link: https://brimark.connieprice.co.uk/activate?hash={1}<br>" +
                    "If you did not expect this email, please disregard it.<br>" +
                    "<br>" +
                    "Warm Regards & Happy Shopping,<br>" +
                    "Brimark<br>", recipient_username, activation_hash),                              
                    IsBodyHtml = true,
                };
                message.To.Add(recipient);

                client.Send(message);
                return true;

            } catch (SmtpException e)
            {
                return false;   
            }
        

        }
        

    }
}
