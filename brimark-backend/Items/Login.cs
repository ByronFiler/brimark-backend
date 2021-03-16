using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace brimark_backend
{
    /* The User's personal account with all of their private settings and info */
    public class Login : Account
    {
        public string Email { get; set; }

        public string PaymentInformation {get; set;}

        public bool DarkTheme { get; set; }

    }
}

