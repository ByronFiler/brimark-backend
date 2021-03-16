using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Linq;

// TODO: Determine Valid IPs or however we want to do that
// TODO: Validate and connect to database
namespace brimark_backend.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class AccountController : ControllerBase
    {
        // TODO Yes this could be done as one statement but using IN seems to be tricky to sanitize, look at doing this in future
        private static readonly MySqlCommand findAccount = new MySqlCommand("SELECT profile_picture, name, account_created FROM `accounts` WHERE name=@name;");
        private static readonly MySqlParameter nameParameter = new MySqlParameter("@name", MySqlDbType.VarChar, 32);

        private readonly ILogger<AccountController> _logger;

        public AccountController(ILogger<AccountController> logger)
        {
            _logger = logger;
        }

        [HttpPost]
        public IEnumerable<Accounts> Post(string[] names)
        {
            
            if (names != null)
            {

                return Enumerable.Range(0, names.Length).Select(i =>
                {
                    string name = names[i];

                    if (Utils.Validate.IsAlphanumerical(name) && name.Length <= 32)
                    {

                        try
                        {
                            nameParameter.Value = name;
                            findAccount.Parameters.Clear();
                            findAccount.Parameters.Add(nameParameter);
                            findAccount.Prepare();

                            _logger.LogInformation(findAccount.CommandText);


                            using (MySqlDataReader accountData = findAccount.ExecuteReader())
                            {
                                if (accountData.HasRows && accountData.Read())
                                {
                                    return new Accounts()
                                    {
                                        Account = new Account()
                                        {
                                            ProfilePicture = accountData.GetString("profile_picture"),
                                            Name = accountData.GetString("name"),
                                            AccountCreated = accountData.GetDateTime("account_created"),
                                        },
                                        StatusCode = 200,
                                        Message = "Account Found."
                                    };

                                } else
                                {
                                    return new Accounts()
                                    {
                                        Account = null,
                                        StatusCode = 204,
                                        Message = "No Matching Account Found."
                                    };
                                }
                        }
                        } catch (MySqlException e)
                        {

                            _logger.LogError(e.Message);

                            return new Accounts()
                            {
                                Account = null,
                                StatusCode = 500,
                                Message = "Internal Server Error, please try again later."
                            };
                        }
                        

                    } else
                    {
                        return new Accounts()
                        { 
                            Account = null,
                            StatusCode = 400,
                            Message = "Invalid Username."
                        };

                    }


                });

            } else
            {
                // 400: Bad Request (Invalid Parameters)
                this.HttpContext.Response.StatusCode = 400;
                return null;
            }
        }

        public static void SetConnection(MySqlConnection connection)
        {
            findAccount.Connection = connection;
        }
    }

}
