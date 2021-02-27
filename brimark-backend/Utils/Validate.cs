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
            return new Regex("^[a-zA-Z0-9]*$").IsMatch(id) && id.Length == 8;
        }

    }
}
