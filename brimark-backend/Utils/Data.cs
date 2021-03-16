using Newtonsoft.Json;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;

namespace brimark_backend.Utils
{
    public static class Data
    {
        private static readonly Dictionary<string, string> email;
        private static readonly Dictionary<string, string> database;
        private static readonly Dictionary<string, string> paypal;

        static Data()
        {
            try
            {
                using (StreamReader data = new StreamReader("data.json"))
                {
                    string json = data.ReadToEnd();
                    Dictionary<string, Dictionary<string, string>> items = JsonConvert.DeserializeObject<Dictionary<string, Dictionary<string, string>>>(json);

                    email = items["email"];
                    database = items["database"];
                    paypal = items["paypal"];
                }
            } catch (IOException e)
            {
                Debug.WriteLine("[FATAL] Config file not found!");
                System.Environment.Exit(1);
            }
            
        }

        public static Dictionary<string, string> GetEmail()
        {
            return email;
        }

        public static Dictionary<string, string> GetDatabase()
        {
            return database;
        }

        public static Dictionary<string, string> GetPayPal()
        {
            return paypal;
        }


        


    }
}
