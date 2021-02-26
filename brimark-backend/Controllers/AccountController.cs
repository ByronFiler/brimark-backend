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
        private readonly ILogger<AccountController> _logger;

        public AccountController(ILogger<AccountController> logger)
        {
            _logger = logger;
        }

        [HttpGet]
        public IEnumerable<Account> Get()
        {
            return Enumerable.Range(1, 5).Select(Index => new Account
            {
                ProfilePicture = DataGenerator.MakeId(),
                Name = DataGenerator.MakeName(),
                AccountCreated = DataGenerator.MakeDate(2015),
                ItemsSold = DataGenerator.GetRng().Next(250),
                SellerRating = (sbyte)DataGenerator.GetRng().Next(6, 10),
                CountryCode = "GB",
            }).ToArray();
        }

    }
}
