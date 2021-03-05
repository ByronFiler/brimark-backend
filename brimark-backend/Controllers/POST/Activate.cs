using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using MySql.Data.MySqlClient;
using System;
using System.Text;

namespace brimark_backend.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class Activate : ControllerBase
    {

        private readonly static MySqlCommand activateAccountSql = new MySqlCommand(
            @"UPDATE `accounts` SET activated=1 WHERE activation_hash=@activation_hash;"
            );
        private readonly static MySqlCommand checkHash = new MySqlCommand(
            @"SELECT * FROM `accounts` WHERE activation_hash=@activation_hash AND activated=1;"
            );

        private static readonly MySqlParameter activationHashParameter = new MySqlParameter("@activation_hash", MySqlDbType.VarChar, 32);

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
                try
                {
                    activationHashParameter.Value = hash;
                    activateAccountSql.Parameters.Add(activationHashParameter);

                    activateAccountSql.Prepare();
                    int effectedRows = activateAccountSql.ExecuteNonQuery();

                    if (effectedRows == 0)
                    {
                        // Did not activate, does an account exist

                        checkHash.Parameters.Add(activationHashParameter);

                        using (MySqlDataReader activatedAccount = checkHash.ExecuteReader())
                        {

                            if (activatedAccount.HasRows)
                            {
                                byte[] alreadyActivatedBody = Encoding.UTF8.GetBytes(String.Format(responseBody, "ALREADY_ACTIVATED"));
                                Response.ContentType = "application/json";
                                Response.Body.Write(alreadyActivatedBody, 0, alreadyActivatedBody.Length);

                                // 204: No Content (Account Already Activated)
                                return StatusCode(204);
                            } else
                            {
                                byte[] noMatchingAccountBody = Encoding.UTF8.GetBytes(String.Format(responseBody, "NO_MATCHING_ACCOUNT"));
                                Response.ContentType = "application/json";
                                Response.Body.Write(noMatchingAccountBody, 0, noMatchingAccountBody.Length);

                                // 204: No Content (No Matching Account)
                                return StatusCode(204);
                            }
                        }

                    }
                    else
                    {
                        // Activated successfully
                        return StatusCode(200);
                    }

                    // Should return: internal failure, activated, already activated, no matching account (assume that the thingy is already checked as valid)
                }
                catch (MySqlException e)
                {
                    // 500: Internal Server Error (Database Failure)
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
