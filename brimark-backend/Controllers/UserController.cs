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
        private readonly ILogger<UserController> _logger;

        public UserController(ILogger<UserController> logger)
        {
            _logger = logger;
        }

        [HttpGet]
        public IEnumerable<User> Get()
        {
            return Enumerable.Range(1, 5).Select(Index => new User
            {
                /* Account Data */
                ProfilePicture = DataGenerator.MakeId(),
                Name = DataGenerator.MakeName(),
                AccountCreated = DataGenerator.MakeDate(2015),
                ItemsSold = DataGenerator.GetRng().Next(250),
                SellerRating = (sbyte)DataGenerator.GetRng().Next(6, 10),
                CountryCode = "GB",

                /* User Data */
                Email = DataGenerator.MakeEmail(),
                PaymentInformation = DataGenerator.MakeEmail(),
                DarkTheme = DataGenerator.GetRng().Next(0, 1) == 0,
                Favourites = Enumerable.Range(0, DataGenerator.GetRng().Next(15)).Select(Index =>DataGenerator.MakeId()).ToArray(),
                Transactions = Enumerable.Range(0, DataGenerator.GetRng().Next(7)).Select(Index => DataGenerator.MakeId()).ToArray()

            }).ToArray();
        }

    }
}
