using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;

// TODO: Determine Valid IPs or however we want to do that
// TODO: Validate and connect to database
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

        [HttpPost]
        public Account Post(String id)
        {
            // Future
            bool validIP = true;

            if (validIP)
            { 
                if (Utils.Validate.IdIsValid(id))
                {
                    // OK: 200
                    this.HttpContext.Response.StatusCode = 200;
                    return new Account()
                    {
                        ProfilePicture = DataGenerator.MakeId(),
                        Name = DataGenerator.MakeName(),
                        AccountCreated = DataGenerator.MakeDate(2015),
                        ItemsSold = DataGenerator.GetRng().Next(250),
                        SellerRating = (sbyte)DataGenerator.GetRng().Next(6, 10),
                        CountryCode = "GB",
                    };   
                }
                else
                {
                    // Invalid Format: 400
                    this.HttpContext.Response.StatusCode = 400;
                    return null;
                }
            } else
            {
                // Forbidden: 403
                this.HttpContext.Response.StatusCode = 403;
                return null;
            }

            
        }



    }
}
