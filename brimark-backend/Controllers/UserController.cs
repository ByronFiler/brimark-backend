using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace brimark_backend.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class UserController : ControllerBase
    {

        private static readonly string[] TestNames = new[]
        {
            "Byron", "Jordan", "Dan", "Connie", "Munashe", "Humza"
        };

        private readonly ILogger<UserController> _logger;

        public UserController(ILogger<UserController> logger)
        {
            _logger = logger;
        }

        [HttpGet]
        public IEnumerable<User> Get()
        {
            // Generating Dummy Data
            var rng = new Random();

            // Profile Picture
            string profilePicture = new string(Enumerable.Repeat("ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789", 8).Select(s => s[rng.Next(s.Length)]).ToArray());

            // Name
            string name = TestNames[rng.Next(TestNames.Length)];

            // AccountCreated
            DateTime start = new DateTime(2015, 1, 1);
            int range = (DateTime.Today - start).Days;
            DateTime accountCreated = start.AddDays(new Random().Next(range));

            // ItemsSold
            int itemsSold = rng.Next(250);

            // SellerRating
            sbyte sellerRating = (sbyte)rng.Next(6, 10);

            // Country Code
            string countryCode = "GB";

            // Email
            string email = name + rng.Next(300) + "@gmail.com";

            // Payment Information
            string paymentInformation = email;

            // Dark Theme
            bool darkTheme = rng.Next(0, 1) == 0;

            // Favourites
            string[] favourites = Enumerable
                .Range(0, rng.Next(15))
                .Select(
                    Index => new string(
                        Enumerable
                        .Repeat("ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789", 8)
                        .Select(s => s[rng.Next(s.Length)]).ToArray())
                    ).ToArray();

            string[] transactions = Enumerable
                .Range(0, rng.Next(7))
                .Select(
                    Index => new string(
                        Enumerable
                        .Repeat("ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789", 8)
                        .Select(s => s[rng.Next(s.Length)]).ToArray())
                    ).ToArray();


            return Enumerable.Range(1, 5).Select(Index => new User
            {
                /* Account Data */
                ProfilePicture = profilePicture,
                Name = name,
                AccountCreated = accountCreated,
                ItemsSold = itemsSold,
                SellerRating = sellerRating,
                CountryCode = countryCode,

                /* User Data */
                Email = email,
                PaymentInformation = paymentInformation,
                DarkTheme = darkTheme,
                Favourites = favourites,
                Transactions = transactions

            }).ToArray();
        }

    }
}
