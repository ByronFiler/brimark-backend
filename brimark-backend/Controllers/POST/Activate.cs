using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Text;

namespace brimark_backend.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class Activate : ControllerBase
    {
        private readonly ILogger<Activate> _logger;

        private static readonly string responseBody = "{\"response\":\"{0}\"";
       
        public Activate(ILogger<Activate> logger)
        {
            _logger = logger;
        }

        [HttpPost]
        public StatusCodeResult Post(String hash)
        {

            // Still needs a valid IP, as it will be accessed by a clean page that will use this
            if (
                Utils.Validate.IsAlphanumerical(hash) && hash.Length == 32
                )
            {
                switch (Utils.Database.POST.activate(hash))
                {
                    case Utils.Status.OK:

                        // 201: Created (Account Activated)
                        return StatusCode(201);

                    case Utils.Status.ALREADY_ACTIVATED:

                        byte[] alreadyActivatedBody = Encoding.UTF8.GetBytes(String.Format(responseBody, "ALREADY_ACTIVATED"));
                        Response.ContentType = "application/json";
                        Response.Body.Write(alreadyActivatedBody, 0, alreadyActivatedBody.Length);

                        // 204: No Content (Account Already Activated)
                        return StatusCode(204);

                    case Utils.Status.NO_MATCHING_ACCOUNT:

                        byte[] noMatchingAccountBody = Encoding.UTF8.GetBytes(String.Format(responseBody, "NO_MATCHING_ACCOUNT"));
                        Response.ContentType = "application/json";
                        Response.Body.Write(noMatchingAccountBody, 0, noMatchingAccountBody.Length);

                        // 204: No Content (No Matching Account)
                        return StatusCode(204);

                    case Utils.Status.DATABASE_FAILURE:

                        // 500: Internal Server Error (Database Failure)
                        return StatusCode(500);

                    default:

                        // 500: Internal Server Error (Unimplemented Status, Should Never Happen)
                        return StatusCode(500);
                }
            } else
            {
                // 400: Bad Request (Invalid Parameters)
                return StatusCode(400);
            }  

        }

    }
}
