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
    public class ItemController : ControllerBase
    {
        private readonly ILogger<ItemController> _logger;

        public ItemController(ILogger<ItemController> logger)
        {
            _logger = logger;
        }

        [HttpGet]
        public IEnumerable<Item> Get()
        {
            // Listing Name
            Dictionary<string, string> item = DataGenerator.MakeItem();
            Array values = Enum.GetValues(typeof(Item.Condition));
            float originalPrice = DataGenerator.GetRng().Next(100, 10000) / 100;

            Item.Condition condt = (Item.Condition)values.GetValue(DataGenerator.GetRng().Next(values.Length));
            _logger.LogDebug("test");

            return Enumerable.Range(1, 5).Select(Index => new Item
            {
                ID = DataGenerator.MakeId(),
                ListingName = item["name"],
                Manufacturer = item["manufacturer"],
                Size = item["size"],
                ItemCondition = (Item.Condition) values.GetValue(DataGenerator.GetRng().Next(values.Length)),
                Price = originalPrice - (originalPrice * (DataGenerator.GetRng().Next(0, 40) / 100) ),
                OriginalPrice = originalPrice,
                DeliveryPrice = DataGenerator.GetRng().Next(10) / 2,
                DeliveryCountriesAccepted = new string[]{"gb"},
                TimeListed = DataGenerator.MakeDate(2019),
                Owner = DataGenerator.MakeId(),
                Images = Enumerable.Range(0, DataGenerator.GetRng().Next(7)).Select(Index => DataGenerator.MakeId()).ToArray(),
                Description = DataGenerator.MakeDescription(),
            }).ToArray();
        }
    }
}
