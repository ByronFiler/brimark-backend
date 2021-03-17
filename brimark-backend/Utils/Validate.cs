using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Text.RegularExpressions;
using System.Diagnostics;

namespace brimark_backend.Utils
{
    public class Validate
    {
        // Alphanumerical & Length of 8
        public static bool IdIsValid(String id)
        {
            return IsAlphanumerical(id) && id.Length == 8;
        }

        public static bool IsAlphanumerical(String text)
        {
            return new Regex(@"^[a-zA-Z0-9]*$").IsMatch(text);
        }

        public static EmailStates IsValidEmail(String email)
        {
            try
            {
                var addr = new System.Net.Mail.MailAddress(email);

                if (addr.Address == email)
                {
                    return new Regex(@"(\.edu(\.[a-z]+)?|\.ac\.[a-z]+)$").IsMatch(addr.Address) ? EmailStates.VALID : EmailStates.NOT_EDU_EMAIL;
                }
                else return EmailStates.INVALID_EMAIL;
                
            }
            catch (FormatException)
            {
                return EmailStates.INVALID_EMAIL;
            }
        }

        /*
         * Password Requirements, At Least One:
         *  - uppercase letter
         *  - lowercase letter
         *  - number
         *  - special character
         */
        public static bool IsValidPassword(String password)
        {
            return new Regex(@"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)(?=.*[@$!%*?&])[A-Za-z\d@$!%*?&]{8,64}$").IsMatch(password);
        }

        public enum EmailStates
        {
            VALID,
            INVALID_EMAIL,
            NOT_EDU_EMAIL
        }

    }

    
}
