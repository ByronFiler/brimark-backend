using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

/*
 * Viewing a user's profile that isn't ones own
 */

namespace brimark_backend
{
    public class Account
    {
        /* 8 alphanumerical characters: https://[domain]/profiles/[ProfilePicture].png */
        public string ProfilePicture { get; set; }

        public string Name { get; set; }

        public DateTime AccountCreated { get; set; }

        public int ItemsSold { get; set; }

        public sbyte SellerRating { get; set; }

        public string CountryCode { get; set; }
    }
}
