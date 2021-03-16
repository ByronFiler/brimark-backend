using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace brimark_backend.Utils.Session
{
    public class SessionManager
    {

        Dictionary<string, Session> sessions = new Dictionary<string, Session>();

        // new key (account name)
        public void newKey(string accountName)
        {

            sessions.Add(accountName, new Session());

        }

        // check key (key) 
        public bool checkKey(string key)
        {
            return sessions.ContainsKey(key) && sessions[key].reauthorizationDate >= DateTime.Now;
        }

        // reauthorize (key) 
        public void reauthorize(string key)
        {
            sessions[key].reauthorizationDate = DateTime.Now.AddHours(1);
        }


        // kill (key)
        public void kill(string key)
        {
            sessions.Remove(key);
        }

    }
}
