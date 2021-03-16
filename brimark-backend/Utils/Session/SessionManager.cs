using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace brimark_backend.Utils.Session
{
    public class SessionManager
    {

        private readonly Dictionary<string, Session> sessions = new Dictionary<string, Session>();

        public void newKey(string accountName)
        {

            sessions.Add(accountName, new Session());

        }

        public bool checkKey(string key)
        {
            return sessions.ContainsKey(key) && sessions[key].reauthorizationDate >= DateTime.Now;
        }

        public void reauthorize(string key)
        {
            sessions[key].reauthorizationDate = DateTime.Now.AddHours(1);
        }


        public void kill(string key)
        {
            sessions.Remove(key);
        }

    }
}
