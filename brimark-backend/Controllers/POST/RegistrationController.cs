using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using MySql.Data.MySqlClient;

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

        private static readonly MySqlCommand createAccountSql = new MySqlCommand(
            @"INSERT INTO `accounts` (name, password, email, activation_hash, profile_picture) VALUES (@name, @password, @email, @activation_hash, @profile_picture);"
            );
        private static readonly MySqlCommand delete = new MySqlCommand(
            @"INSERT INTO `emails` (accountId) VALUES ((SELECT id FROM `accounts` WHERE id = (SELECT MAX(id) FROM `accounts`)));"
            );

        private static readonly MySqlCommand checkExistingNames = new MySqlCommand(
            @"SELECT * FROM `accounts` WHERE name=@name;"
            );
        private static readonly MySqlCommand checkExistingEmail = new MySqlCommand(
            @"SELECT * FROM `accounts` WHERE email=@email;"
            );

        private static readonly MySqlParameter nameParameter = new MySqlParameter("@name", MySqlDbType.VarChar, 32);
        private static readonly MySqlParameter passwordParameter = new MySqlParameter("@password", MySqlDbType.VarChar, 64);
        private static readonly MySqlParameter emailParameter = new MySqlParameter("@email", MySqlDbType.VarChar, 64);
        private static readonly MySqlParameter activationHashParameter = new MySqlParameter("@activation_hash", MySqlDbType.VarChar, 32);
        private static readonly MySqlParameter profilePictureParameter = new MySqlParameter("@profile_picture", MySqlDbType.VarChar, 10);

        private static readonly string responceBody = "{{\"error\": \"duplicate_data\", \"duplicates\": [{0}]}}";
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
                try
                {
                    List<string> duplicates = new List<string>();

                    nameParameter.Value = username;

                    checkExistingNames.Parameters.Clear();
                    checkExistingNames.Parameters.Add(nameParameter);

                    using (MySqlDataReader checkAccounts = checkExistingNames.ExecuteReader())
                    {
                        if (checkAccounts.HasRows) duplicates.Append("\"username\"");
                    }

                    emailParameter.Value = email;

                    checkExistingEmail.Parameters.Clear();
                    checkExistingEmail.Parameters.Add(emailParameter);

                    using (MySqlDataReader checkEmail = checkExistingEmail.ExecuteReader())
                    {
                        if (checkEmail.HasRows) duplicates.Append("\"email\"");
                    }

                    if (duplicates.Count > 0)
                    {

                        // Writing Body Message
                        byte[] bothDuplicatesBody = Encoding.UTF8.GetBytes(String.Format(responceBody, String.Join(", ", duplicates)));
                        Response.ContentType = "application/json";
                        Response.Body.Write(bothDuplicatesBody, 0, bothDuplicatesBody.Length);

                        // 206: Not Acceptable (Duplicate Email)
                        return StatusCode(206);

                    } else
                    {
                        passwordParameter.Value = Utils.Encryption.hash(password);
                        profilePictureParameter.Value = "PF" + DataGenerator.MakeId();
                        activationHashParameter.Value = DataGenerator.MakeHash();

                        createAccountSql.Parameters.Clear();
                        createAccountSql.Parameters.Add(nameParameter);
                        createAccountSql.Parameters.Add(passwordParameter);
                        createAccountSql.Parameters.Add(emailParameter);
                        createAccountSql.Parameters.Add(profilePictureParameter);
                        createAccountSql.Parameters.Add(activationHashParameter);

                        createAccountSql.Prepare();
                        createAccountSql.ExecuteNonQuery();

                        delete.Prepare();
                        delete.ExecuteNonQuery();

                        Utils.Mail.MailManager.Poke();

                        // 201: Created (Account Created)
                        return StatusCode(201);
                    }
                    

                } catch (MySqlException e)
                {

                    _logger.LogError("Database Failure when Attempting to Create Account");
                    _logger.LogError("   -> " + e.Message);
                    // 500: Internal Server Error (Database Failure)
                    return StatusCode(500);
                }
                
            } else
            {
                // 400: Bad Request (Invalid Form Data)
                return StatusCode(400);
            }
            
        }

        public static void SetConnection(MySqlConnection connection)
        {
            createAccountSql.Connection = connection;
            delete.Connection = connection;
            checkExistingNames.Connection = connection;
            checkExistingEmail.Connection = connection;

        }

    }
}
