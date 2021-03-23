using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace brimark_backend.Utils.Session
{
    public class SessionManager
    {
        private static readonly Dictionary<string, Session> sessionsByKey = new Dictionary<string, Session>();
        private static readonly Dictionary<string, Session> sessionsByAccountName = new Dictionary<string, Session>();

        public static string newKey(string accountName)
        {
            // If a session already exists just toss it and create a new one.
            if (sessionsByAccountName.ContainsKey(accountName)) {
                Session oldSession = sessionsByAccountName[accountName];

                sessionsByKey.Remove(oldSession.key);
                sessionsByAccountName.Remove(oldSession.accountName);
            }

            Session createdSession = new Session(accountName);

            sessionsByKey.Add(createdSession.key, createdSession);
            sessionsByAccountName.Add(createdSession.accountName, createdSession);

            return createdSession.key;
        }

        public static bool checkKey(string key)
        {
            if (sessionsByKey.ContainsKey(key)) {
                Session potentialSession = sessionsByKey[key];

                if (potentialSession.isExpired()) {
                    if (sessionsByKey[key].isReauthorizationExpired()) {
                        kill(key);
                    }
                } else {
                    return true;
                }
            }

            return false;
        }

        public static void reauthorize(string key)
        {
            if (sessionsByKey.ContainsKey(key)) {
                sessionsByKey[key].reauthorize();
            }
        }

        public static void kill(string key)
        {
            if (sessionsByKey.ContainsKey(key)) {
                Session sessionToKill = sessionsByKey[key];

                sessionsByKey.Remove(sessionToKill.key);
                sessionsByAccountName.Remove(sessionToKill.accountName);
            }
        }

    }
}
