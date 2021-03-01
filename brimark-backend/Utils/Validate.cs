using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Text.RegularExpressions;

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

        public static bool IsValidEmail(String email)
        {
            try
            {
                var addr = new System.Net.Mail.MailAddress(email);
                return addr.Address == email;
            }
            catch
            {
                return false;
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

    }
}
