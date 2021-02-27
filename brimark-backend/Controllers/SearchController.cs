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
    public class SearchController : ControllerBase
    {

        private readonly ILogger<SearchController> _logger;

        public SearchController(ILogger<SearchController> logger)
        {
            _logger = logger;
        }

        [HttpPost]
        public IEnumerable<Search> Get(String query)
        {
            // Future
            bool validIP = true;
            bool validQuery = true;
            bool foundSearchResults = true; // More than 0 results

            if (validIP)
            {

                if (validQuery)
                {
                    if (foundSearchResults)
                    {
                        // OK: 200
                        this.HttpContext.Response.StatusCode = 200;
                        return Enumerable.Range(1, 10).Select(x =>
                        {
                            Dictionary<String, String> item = DataGenerator.MakeItem();
                            return new Search()
                            {
                                ID = DataGenerator.MakeId(),
                                Image = DataGenerator.MakeId(),
                                Title = item["name"],
                                Price = DataGenerator.GetRng().Next(100, 10000) / 100
                            };
                        });
                    } else
                    {
                        // No Content: 204 (No search results found)
                        this.HttpContext.Response.StatusCode = 204;
                        return null;
                    }
                    
                } else
                {
                    // Bad Request: 400 (Invalid Query)
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

