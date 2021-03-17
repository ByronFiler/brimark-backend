using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace brimark_backend.Utils.Session
{
    public class SessionManager
    {

        private static readonly Dictionary<string, Session> sessions = new Dictionary<string, Session>();

        public static string newKey(string accountName)
        {
            Session createdSession = new Session();
            sessions.Add(accountName, new Session());
            return createdSession.key;

        }

        public static bool checkKey(string key)
        {
            return sessions.ContainsKey(key) && sessions[key].reauthorizationDate >= DateTime.Now;
        }

        public static void reauthorize(string key)
        {
            sessions[key].reauthorizationDate = DateTime.Now.AddHours(1);
        }


        public static void kill(string key)
        {
            sessions.Remove(key);
        }

    }
}
