using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Linq;

// Must USE SSL Connection due to sensitive password
// TODO: Determine Valid IPs or however we want to do that
// TODO: Validate and connect to database
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

        [HttpPost]
        public User Get(String id, String password)
        {
            // Future
            bool validIP = true;
            bool validPw = true;

            if (validIP)
            {
                if (Utils.Validate.IdIsValid(id))
                {
                    if (validPw)
                    {
                        return new User()
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
                            Favourites = Enumerable.Range(0, DataGenerator.GetRng().Next(15)).Select(Index => DataGenerator.MakeId()).ToArray(),
                            Transactions = Enumerable.Range(0, DataGenerator.GetRng().Next(7)).Select(Index => DataGenerator.MakeId()).ToArray()

                        };
                    }
                    else
                    {
                        // Unauthorized: 401 (Invalid password given)
                        this.HttpContext.Response.StatusCode = 401;
                        return null;
                    }
                }
                else
                {
                    // Invalid Format: 400 (Id incorrectly formatted)
                    this.HttpContext.Response.StatusCode = 400;
                    return null;
                }
            }
            else
            {
                // Forbidden: 403 (Access from non-whitelisted IP)
                this.HttpContext.Response.StatusCode = 403;
                return null;
            }
        }
    }
}
