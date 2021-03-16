using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using MySql.Data.MySqlClient;
using System;
using System.Linq;
using System.Text;

// Must USE SSL Connection due to sensitive password
// TODO: Determine Valid IPs or however we want to do that
// TODO: Validate and connect to database
namespace brimark_backend.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class LoginController : ControllerBase
    {
        private readonly ILogger<LoginController> _logger;

        private static readonly string responseBody = "{{\"error\":\"{0}\"}}";

        private static readonly MySqlCommand loginWithEmail = new MySqlCommand("SELECT * FROM `accounts` WHERE email=@email AND password=@password;");
        private static readonly MySqlCommand loginWithUsername = new MySqlCommand("SELECT * FROM `accounts` WHERE name=@name AND password=@password;");

        private static readonly MySqlParameter nameParameter = new MySqlParameter("@name", MySqlDbType.VarChar, 32);
        private static readonly MySqlParameter emailParameter = new MySqlParameter("@email", MySqlDbType.VarChar, 64);
        private static readonly MySqlParameter passwordParameter = new MySqlParameter("@password", MySqlDbType.VarChar, 64);

        public LoginController(ILogger<LoginController> logger)
        {
            _logger = logger;
        }

        [HttpPost]
        public Login Get(String usernameOrEmail, String password)
        {
            MySqlDataReader loginReader;
            try
            {
                if (Utils.Validate.IsValidEmail(usernameOrEmail))
                {
                    // Login with email
                    emailParameter.Value = usernameOrEmail;
                    passwordParameter.Value = Utils.Encryption.hash(password);

                    loginWithEmail.Parameters.Clear();
                    loginWithEmail.Parameters.Add(emailParameter);
                    loginWithEmail.Parameters.Add(passwordParameter);

                    loginReader = loginWithEmail.ExecuteReader();
                }
                else
                {
                    // Logging in with Username
                    nameParameter.Value = usernameOrEmail;
                    passwordParameter.Value = Utils.Encryption.hash(password);

                    loginWithUsername.Parameters.Add(nameParameter);
                    loginWithUsername.Parameters.Add(passwordParameter);

                    loginReader = loginWithUsername.ExecuteReader();
                }

            } catch (MySqlException e)
            {
                // 500: Internal Server Error (Database Error)
                this.HttpContext.Response.StatusCode = 500;
                return null;
            }

            
            if (loginReader.HasRows && loginReader.Read())
            {
                if (loginReader.GetBoolean("activated"))
                {
                    // User has activated their account

                    // Providing Userdata
                    this.HttpContext.Response.StatusCode = 200;

                    return new Login()
                    {

                        ProfilePicture = loginReader.GetString("profile_picture"),
                        Name = loginReader.GetString("name"),
                        AccountCreated = loginReader.GetDateTime("account_created"),

                        Email = loginReader.GetString("email"),
                        PaymentInformation = loginReader.GetString("payment_email"),
                        DarkTheme = loginReader.GetBoolean("dark_theme"),

                    };

                } else
                {
                    // User has not activated their account do not allow them to login

                    // 403: Forbidden (Has not activated their account)
                    this.HttpContext.Response.StatusCode = 403;

                    // Prepare body response informing
                    byte[] hasNotActivatedBody = Encoding.UTF8.GetBytes(String.Format(responseBody, "HAS_NOT_ACTIVATED"));
                    Response.ContentType = "application/json";
                    Response.Body.Write(hasNotActivatedBody, 0, hasNotActivatedBody.Length);
                    return null;
                }
            } else
            {
                // Forbidden: 403 (Invalid Username and Password)
                this.HttpContext.Response.StatusCode = 403;

                // Responce Body
                byte[] invalidCredentialsBody = Encoding.UTF8.GetBytes(String.Format(responseBody, "INVALID_CREDENTIALS"));
                Response.ContentType = "application/json";
                Response.Body.Write(invalidCredentialsBody, 0, invalidCredentialsBody.Length);
                return null;
            }
        }

        public static void SetConnection(MySqlConnection connection)
        {
            loginWithEmail.Connection = connection;
            loginWithUsername.Connection = connection;
        }
    }
}
