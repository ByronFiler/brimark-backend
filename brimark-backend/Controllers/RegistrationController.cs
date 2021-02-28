using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

// Must USE SSL Connection due to sensitive password
// TODO: Determine Valid IPs or however we want to do that
// TODO: Validate and connect to database
// Handle Null values being posted
namespace brimark_backend.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class RegistrationController : ControllerBase
    {

        string ip = "127.0.0.1";
        bool validIp = true;
        bool successfulDatabase = true;

        private readonly ILogger<RegistrationController> _logger;

        public RegistrationController(ILogger<RegistrationController> logger)
        {
            _logger = logger;
        }

        [HttpPost]
        public StatusCodeResult Post(
            String username,
            String email,
            String password
            )
        {
            if (validIp)
            {
                // Validate data on Client Side & Server Side as Client Side can be Manipulated
                if (
                        Utils.Validate.IsAlphanumerical(username) && username.Length <= 16
                        && Utils.Validate.IsValidEmail(email) && email.Length <= 64
                        && Utils.Validate.IsValidPassword(password)
                    )
                {
                    string ActivationHash = BitConverter.ToString(MD5.Create().ComputeHash(Guid.NewGuid().ToByteArray())).Replace("-", "").ToLower();

                    // Write to database
                    if (successfulDatabase)
                    {
                        // Send Validation Email

                        // 201: Created (Account created in database)

                        return StatusCode(201);

                    } else
                    {
                        // 500: Internal Server Error (Database Failure)
                        _logger.LogError("Database Failure when Attempting to Create Account");
                        return StatusCode(500);

                    }

                } else
                {
                    // 400: Bad Request (Invalid Form Data)
                    _logger.LogWarning(String.Format("Invalid Data Given from Whitelisted IP ({0}), user either manipulating form or a form has an error.", ip));
                    return StatusCode(400);
                }
            } else
            {
                // 403: Forbidden (Non Whitelisted IP)
                _logger.LogWarning("Access attempted non whitelisted ip: " + ip);
                return StatusCode(403);
            }
            
            
        }

    }
}
