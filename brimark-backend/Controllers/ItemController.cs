using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;

// TODO: Determine Valid IPs or however we want to do that
// TODO: Validate and connect to database
namespace brimark_backend.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class ItemController : ControllerBase
    {
        private readonly ILogger<ItemController> _logger;

        public ItemController(ILogger<ItemController> logger)
        {
            _logger = logger;
        }

        [HttpPost]
        public Item Get(String id)
        {
            // Future
            bool validIP = true;

            // Listing Name
            Dictionary<string, string> item = DataGenerator.MakeItem();
            Array values = Enum.GetValues(typeof(Item.Condition));
            float originalPrice = DataGenerator.GetRng().Next(100, 10000) / 100;

            Item.Condition condt = (Item.Condition)values.GetValue(DataGenerator.GetRng().Next(values.Length));
            _logger.LogDebug("test");

            if (validIP)
            {
                if (Utils.Validate.IdIsValid(id))
                {
                    // OK: 200
                    this.HttpContext.Response.StatusCode = 200;
                    return new Item()
                    {
                        ID = DataGenerator.MakeId(),
                        ListingName = item["name"],
                        Manufacturer = item["manufacturer"],
                        Size = item["size"],
                        ItemCondition = (Item.Condition)values.GetValue(DataGenerator.GetRng().Next(values.Length)),
                        Price = originalPrice - (originalPrice * (DataGenerator.GetRng().Next(0, 40) / 100)),
                        OriginalPrice = originalPrice,
                        DeliveryPrice = DataGenerator.GetRng().Next(10) / 2,
                        DeliveryCountriesAccepted = new string[] { "gb" },
                        TimeListed = DataGenerator.MakeDate(2019),
                        Owner = DataGenerator.MakeId(),
                        Images = Enumerable.Range(0, DataGenerator.GetRng().Next(7)).Select(Index => DataGenerator.MakeId()).ToArray(),
                        Description = DataGenerator.MakeDescription(),
                    };
                }
                else
                {
                    // Invalid Format: 400 (Id incorrectly formatted)
                    this.HttpContext.Response.StatusCode = 400;
                    return null;
                }
            } else
            {
                // Forbidden: 403 (Access from non-whitelisted IP)
                this.HttpContext.Response.StatusCode = 403;
                return null;
            }
        }
    }
}
