using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;

namespace brimark_backend.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class Activate : ControllerBase
    {

        private readonly ILogger<Activate> _logger;

        public Activate(ILogger<Activate> logger)
        {
            _logger = logger;
        }

        [HttpPost]
        public StatusCodeResult Post(String hash)
        {

            // Still needs a valid IP, as it will be accessed by a clean page that will use this
            bool validIp = true;
            bool workingDatabase = true;
            bool validRequest = true;

            if (validIp)
            {

                if (
                    Utils.Validate.IsAlphanumerical(hash) && hash.Length == 32
                    )
                {

                    if (workingDatabase)
                    {
                        if (validRequest)
                        {
                            // 201: Created (Account Activated)
                            return StatusCode(201);
                        }
                        else
                        {
                            // 403: Forbidden (Account activation timeout window passed)
                            // Should probs inform user here too?
                            return StatusCode(403);
                        }
                    }
                    else
                    {
                        // 500: Internal Server Error (Databas Failure)
                        return StatusCode(500);
                    }

                } else
                {
                    // Invalid Request: 400 (Invalid Parameters)
                    return StatusCode(400);

                }
            } else
            {
                // Forbidden: 403 (Non Whitelisted-IP)
                return StatusCode(403);
            }


        }

    }
}
