using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

// Could Auto-Convert Sizing?

namespace brimark_backend
{
    public class Item
    {
        public String ID { set; get; }

        public string ListingName { set; get; }

        public string Manufacturer { set; get; }

        public string Size { set; get; }

        public Condition ItemCondition { set; get; }

        public float Price { set; get; }

        public float OriginalPrice { set; get; }

        public float DeliveryPrice { set; get; }

        public string[] DeliveryCountriesAccepted { set; get; }

        public DateTime TimeListed { set; get; }

        public string Owner { set; get; }

        public string[] Images { set; get; }

        public string Description { set; get; }

        // https://theclothingvault.com/condition-guide/
        public enum Condition
        {
            NEW_WITH_TAGS,
            NEW_WITHOUT_TAGS,
            DEADSTOCK,
            LIKE_NEW,
            EXCELLENT,
            VERY_GOOD,
            DISTRESSED
        }

    }
}
