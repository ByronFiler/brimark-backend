﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace brimark_backend.Utils.Mail
{
    public class Mail
    {
        public string UserEmail { 
            get { return UserEmail; }
            set { UserEmail = value; }
        }
        public string Hash { 
            get { return Hash; }
            set { Hash = value; }
        }
        public Mail(String userEmail, String hash)
        {
            this.UserEmail = userEmail;
            this.Hash = hash;
        }
    }
}
