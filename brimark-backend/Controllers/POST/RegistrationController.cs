using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
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
        private static readonly string responceBody = "{\"error\": \"duplicate_data\", \"duplicates\": [{0}]}";
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
            
            // Validate data on Client Side & Server Side as Client Side can be Manipulated
            if (
                    Utils.Validate.IsAlphanumerical(username) && username.Length <= 16
                    && Utils.Validate.IsValidEmail(email) && email.Length <= 64
                    && Utils.Validate.IsValidPassword(password)
                )
            {
                switch (Utils.Database.POST.CreateAccount(username, password, email))
                {
                    case Utils.Status.OK:
                        // 201: Created (Account Created)
                        return StatusCode(201);

                    case Utils.Status.DUPLICATE_EMAIL:

                        // Writing Body Message
                        byte[] emailDuplicatedBody = Encoding.UTF8.GetBytes(String.Format(responceBody, "email"));
                        Response.ContentType = "application/json";
                        Response.Body.Write(emailDuplicatedBody, 0, emailDuplicatedBody.Length);

                        // 206: Not Acceptable (Duplicate Email)
                        return StatusCode(206);

                    case Utils.Status.DUPLICATE_USERNAME:

                        // Writing Body Message
                        byte[] usernameDuplicatedBody = Encoding.UTF8.GetBytes(String.Format(responceBody, "username"));
                        Response.ContentType = "application/json";
                        Response.Body.Write(usernameDuplicatedBody, 0, usernameDuplicatedBody.Length);

                        // 206: Not Acceptable (Duplicate Email)
                        return StatusCode(206);

                    case Utils.Status.DUPLICATE_EMAIL_AND_USERNAME:

                        // Writing Body Message
                        byte[] bothDuplicatesBody = Encoding.UTF8.GetBytes(String.Format(responceBody, "email, username"));
                        Response.ContentType = "application/json";
                        Response.Body.Write(bothDuplicatesBody, 0, bothDuplicatesBody.Length);

                        // 206: Not Acceptable (Duplicate Email)
                        return StatusCode(206);

                    case Utils.Status.DATABASE_FAILURE:
                        _logger.LogError("Database Failure when Attempting to Create Account");

                        // 500: Internal Server Error (Database Failure)
                        return StatusCode(500);

                    default:
                        // 500: Internal Server Error (Unimplemented Status, Should Never Happen)
                        return StatusCode(500);
                }
            } else
            {
                // 400: Bad Request (Invalid Form Data)
                return StatusCode(400);
            }
            
            
            
        }

    }
}
