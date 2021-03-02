using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

// developer=brimark
// create JSON and store passwords

namespace brimark_backend.Utils.Database
{
    public static class POST
    {
        // Force Setting Parameters
        private static readonly MySqlCommand createAccountSql = new MySqlCommand(
            "INSERT INTO `accounts` (name, password, email, country_code, activation_hash, profile_picture) VALUES (@name, @password, @email, @country_code, @activation_hash, @profile_picture);" +
            "INSERT INTO `emails` (recipient, username, hash) VALUES (@email, @name, @activation_hash);",
            null
            );
        private readonly static MySqlCommand activateAccountSql = new MySqlCommand(
            "UPDATE `accounts` SET activated=1 WHERE activation_hash=@activation_hash;" +
            "UPDATE `emails` SET sent=1, date_sent=@date WHERE activation_hash=@activation_hash;",
            null
            );
        private readonly static MySqlCommand createListingSql = new MySqlCommand(
            "INSERT INTO `items` () VALUES ()" +
            "UPDATE `users` SET ",
            null
            );

        private static readonly MySqlParameter nameParameter = new MySqlParameter("@name", MySqlDbType.VarChar, 32);
        private static readonly MySqlParameter passwordParameter = new MySqlParameter("@password", MySqlDbType.VarChar, 64);
        private static readonly MySqlParameter emailParameter = new MySqlParameter("@email", MySqlDbType.VarChar, 64);
        private static readonly MySqlParameter countryParameter = new MySqlParameter("@country_code", MySqlDbType.VarChar, 2);

        private static readonly MySqlParameter activationHashParameter = new MySqlParameter("@activation_hash", MySqlDbType.VarChar, 32);

        private static readonly MySqlParameter dateParameter = new MySqlParameter("@date", MySqlDbType.Date, 10);
        
        private static readonly MySqlParameter profilePictureParameter = new MySqlParameter("@profile_picture", MySqlDbType.VarChar, 10);

        public static void SetConnection(MySqlConnection connection)
        {
            POST.createAccountSql.Connection = connection;
            POST.activateAccountSql.Connection = connection;
        }


        // Create Account
        public static bool CreateAccount(
            
            String name,
            String password,
            String email,
            String country

            )
        {
            nameParameter.Value = name;
            passwordParameter.Value = encrypt(password);
            emailParameter.Value = email;
            countryParameter.Value = country;

            profilePictureParameter.Value = "PF" + DataGenerator.MakeId();
            activationHashParameter.Value = DataGenerator.MakeHash();

            createAccountSql.Parameters.Add(nameParameter);
            createAccountSql.Parameters.Add(passwordParameter);
            createAccountSql.Parameters.Add(emailParameter);
            createAccountSql.Parameters.Add(countryParameter);

            createAccountSql.Parameters.Add(profilePictureParameter);
            createAccountSql.Parameters.Add(activationHashParameter);

            createAccountSql.Prepare();
            createAccountSql.ExecuteNonQuery();

            return true;
        }

        public static void activate(String activationHash)
        {
            activationHashParameter.Value = activationHash;
            dateParameter.Value = DateTime.Now.ToString("yyyy-mm-dd");

            activateAccountSql.Parameters.Add(dateParameter);
            activateAccountSql.Parameters.Add(activationHashParameter);

            activateAccountSql.Prepare();
            activateAccountSql.ExecuteNonQuery();
        }

        public static void createListing(
            String name,
            String owner,
            float price,
            String brand,
            String sizing,
            String description
            )
        {



        }


        private static string encrypt(string password)
        {
            using (SHA256 sha256Hash = SHA256.Create())
            {
                byte[] bytes = sha256Hash.ComputeHash(Encoding.UTF8.GetBytes(password));

                StringBuilder builder = new StringBuilder();
                for (int i = 0; i < bytes.Length; i++)
                {
                    builder.Append(bytes[i].ToString("x2"));
                }
                return builder.ToString();

            }
        }

    }
}
