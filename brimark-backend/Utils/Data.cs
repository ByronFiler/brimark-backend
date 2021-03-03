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


        


    }
}
