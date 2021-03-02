using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Threading.Tasks;

namespace brimark_backend
{
    public class DataGenerator
    {
        private static readonly Random rng = new Random();
        private static readonly List<Dictionary<string, string[]>> items = new List<Dictionary<string, string[]>>()
        { 
            new Dictionary<string, string[]>
            {
                { "Items", new string[]{ "T-Shirt", "Shirt", "Overshirt", "Jumper", "Coat", "Harrington Jacket", "Bomber Jacket", "Parka" } },
                { "Sizes", new string[]{"XS", "S", "M", "L", "XL", "XXL"} }
            },
            new Dictionary<string, string[]>
            {
                { "Items", new string[]{ "Jeans", "Trousers", "Cargo Pants", "Joggers" } },
                { "Sizes", new string[]{"28W 30L", "30L 30W", "32L 30W", "32L, 32W", "34L 32W", "32L 34W"}}
            },
            new Dictionary<string, string[]>
            {
                { "Items", new string[]{ "Trainers", "Chelsea Boots", "Combat Boots", "Brogues", "High Heels", "Skate Shoes", "Loafers", "Platform Shoes" } },
                { "Sizes", new string[]{"6", "6.5", "7", "7.5", "8", "8.5", "9", "9.5", "10", "10.5", "11", "11.5", "12"}}
            },
        };

        private static readonly string[] TestNames = new[]
        {
            "Byron", "Jordan", "Dan", "Connie", "Munashe", "Humza"
        };
        private static readonly string[] emailDomains = new[]
        {
            "googlemail.com", "gmail.com", "yahoo.com", "yandex.ru", "aol.com", "outlook.com", "icloud.com"
        };
        private static readonly string[] colours = new string[]
        {
            "White", "Black", "Green", "Purple", "Brown", "Grey", "Blue"
        };
        private static readonly string[] manufacturers = new string[]
        {
            "Unknown", "Nike", "Uniqlo", "H&M", "Zara", "Adidas", "The North Face", "New Balance", "Levi's"
        };
        private static readonly string descriptionText = "Lorem ipsum dolor sit amet, consectetur adipiscing elit. Duis nec arcu dictum, ornare elit et, hendrerit libero. Aenean cursus metus metus, sit amet pretium felis pretium ut. Donec efficitur viverra nibh id fermentum. Etiam vestibulum vehicula est. Nulla quis ullamcorper enim. Aliquam erat volutpat. Sed sed iaculis mauris, nec condimentum ante. Aliquam aliquam dui vitae consectetur pellentesque. Morbi in ante non odio mollis ornare. Quisque pharetra sem est, sed suscipit mauris sagittis ornare. In laoreet non eros gravida tincidunt. Vestibulum eget urna magna. Donec ac nulla risus. Vivamus vestibulum mattis auctor.";

        public static Random GetRng()
        {
            return rng;
        }

        public static string MakeId()
        {
            return new string(Enumerable.Repeat("ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789", 8).Select(s => s[rng.Next(s.Length)]).ToArray());
        }

        public static string MakeName()
        {
            return TestNames[rng.Next(TestNames.Length)];
        }

        public static DateTime MakeDate(int startYear)
        {
            DateTime start = new DateTime(startYear, 1, 1);
            return start.AddDays(new Random().Next((DateTime.Today - start).Days));
        }

        public static string MakeEmail()
        {
            return MakeName() + rng.Next(300) + "@" + emailDomains[rng.Next(emailDomains.Length)];
        }

        public static Dictionary<String, String> MakeItem()
        {
            Dictionary<string, string[]> selectedItem = items[rng.Next(items.Count)];
            return new Dictionary<string, string>
            {
                { "name",  selectedItem["Items"][rng.Next(selectedItem["Items"].Length)]},
                { "size",  selectedItem["Sizes"][rng.Next(selectedItem["Sizes"].Length)]},
                { "colour", colours[rng.Next(colours.Length)] },
                { "manufacturer", manufacturers[rng.Next(manufacturers.Length)]}
            };
        }

        public static string MakeDescription()
        {

            List<String> x = descriptionText.Split(' ').OrderBy(x => rng.Next()).ToArray().ToList();
            x.RemoveRange(0, (int)(x.Count() * 0.7));
            return String.Join(" ", x);

        }

        public static string MakeHash()
        {
            return BitConverter.ToString(MD5.Create().ComputeHash(Guid.NewGuid().ToByteArray())).Replace("-", "").ToLower();
        }



    }
}
