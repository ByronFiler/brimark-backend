using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace brimark_backend.Utils.Session
{
    public class Session
    {
        public string accountName { get; }
        public string key { get; }

        private DateTime reauthorizationDate;
        private DateTime expirationDate;

        public Session(string accountName) {
            this.accountName = accountName;
            this.key = DataGenerator.MakeId();
            this.reauthorizationDate = DateTime.Now.AddMonths(1);
            this.expirationDate = DateTime.Now.AddDays(14);
        }

        public bool isExpired() {
            return this.expirationDate <= DateTime.Now;
        }

        public bool isReauthorizationExpired() {
            return this.reauthorizationDate <= DateTime.Now;
        }

        public void reauthorize() {
            this.reauthorizationDate = DateTime.Now.AddMonths(1);
            this.expirationDate = DateTime.Now.AddDays(14);
        }
    }
}
