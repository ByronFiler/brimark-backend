using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace brimark_backend.Utils.Session
{
    public class Session
    {
        protected string key { set; get; }

        protected DateTime reauthorizationDate { set; get; }

        protected DateTime expirationDate { set; get; }


        // Key 32 varchar
        // reauth = 1h 
        // expiration = 1month

        public Session()
        {
            this.key = DataGenerator.MakeId();
            this.reauthorizationDate = DateTime.Now.AddHours(1);
            this.expirationDate = DateTime.Now.AddMonths(1);
        }

    }
}
