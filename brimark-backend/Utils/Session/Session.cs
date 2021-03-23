using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace brimark_backend.Utils.Session
{
    public class Session
    {
        public string accountName { set; get; }
        public string key { set; get; }

        public DateTime reauthorizationDate { set; get; }
        public DateTime expirationDate { set; get; }

        public Session(string accountName) {
            this.accountName = accountName;
            this.key = DataGenerator.MakeId();
            this.reauthorizationDate = DateTime.Now.AddMonths(1);
            this.expirationDate = DateTime.Now.AddDays(14);
        }

    }
}
