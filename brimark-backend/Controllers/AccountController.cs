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
    public class AccountController : ControllerBase
    {

        private static readonly string[] TestNames = new[]
        {
            "Byron", "Jordan", "Dan", "Connie", "Munashe", "Humza"
        };

        private readonly ILogger<AccountController> _logger;

        public AccountController(ILogger<AccountController> logger)
        {
            _logger = logger;
        }

        [HttpGet]
        public IEnumerable<Account> Get()
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
            sbyte sellerRating = (sbyte) rng.Next(6, 10);

            // Country Code
            string countryCode = "GB";

            return Enumerable.Range(1, 5).Select(Index => new Account
            {
                ProfilePicture = profilePicture,
                Name = name,
                AccountCreated = accountCreated,
                ItemsSold = itemsSold,
                SellerRating = sellerRating,
                CountryCode = countryCode
            }).ToArray();
        }

    }
}
