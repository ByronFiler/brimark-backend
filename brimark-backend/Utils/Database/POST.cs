using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

// developer=brimark
// Create account SQL syntax errors due to some MySqlClient bug, have to do two separate statements for it to work which is annoying

namespace brimark_backend.Utils.Database
{
    public static class POST
    {
        // Force Setting Parameters
        private static readonly MySqlCommand createAccountSql = new MySqlCommand(
            @"INSERT INTO `accounts` (name, password, email, activation_hash, profile_picture) VALUES (@name, @password, @email, @activation_hash, @profile_picture);"
            );
        private static readonly MySqlCommand delete = new MySqlCommand(
            @"INSERT INTO `emails` (accountId) VALUES ((SELECT id FROM `accounts` WHERE id = (SELECT MAX(id) FROM `accounts`)));"
            );

        private readonly static MySqlCommand activateAccountSql = new MySqlCommand(
            @"UPDATE `accounts` SET activated=1 WHERE activation_hash=@activation_hash;"
            );
        private readonly static MySqlCommand checkHash = new MySqlCommand(
            @"SELECT * FROM `accounts` WHERE activation_hash=@activation_hash AND activated=1;"
            );

        private readonly static MySqlCommand createListingSql = new MySqlCommand(
            "INSERT INTO `items` () VALUES ()" +
            "UPDATE `users` SET ",
            null
            );

        private static readonly MySqlParameter nameParameter = new MySqlParameter("@name", MySqlDbType.VarChar, 32);
        private static readonly MySqlParameter passwordParameter = new MySqlParameter("@password", MySqlDbType.VarChar, 64);
        private static readonly MySqlParameter emailParameter = new MySqlParameter("@email", MySqlDbType.VarChar, 64);
        private static readonly MySqlParameter activationHashParameter = new MySqlParameter("@activation_hash", MySqlDbType.VarChar, 32);
        private static readonly MySqlParameter profilePictureParameter = new MySqlParameter("@profile_picture", MySqlDbType.VarChar, 10);

        //private static readonly MySqlParameter dateParameter = new MySqlParameter("@date", MySqlDbType.Date, 10);


        public static void SetConnection(MySqlConnection connection)
        {
            POST.createAccountSql.Connection = connection;
            POST.activateAccountSql.Connection = connection;
            POST.delete.Connection = connection;
        }


        // Create Account
        public static bool CreateAccount(
            String name,
            String password,
            String email

            )
        { 
            nameParameter.Value = name;
            passwordParameter.Value = encrypt(password);
            emailParameter.Value = email;
            profilePictureParameter.Value = "PF" + DataGenerator.MakeId();
            activationHashParameter.Value = DataGenerator.MakeHash();

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

            return true;
        }

        public static Status activate(String activationHash)
        {
            try
            {
                activationHashParameter.Value = activationHash;
                activateAccountSql.Parameters.Add(activationHashParameter);

                activateAccountSql.Prepare();
                int effectedRows = activateAccountSql.ExecuteNonQuery();

                if (effectedRows == 0)
                {
                    // Did not activate, does an account exist

                    checkHash.Parameters.Add(activationHash);

                    using (MySqlDataReader activatedAccount = checkHash.ExecuteReader())
                    {
                        return activatedAccount.Read() ? Status.ALREADY_ACTIVATED : Status.NO_MATCHING_ACCOUNT;
                    }

                }
                else
                {
                    // Activated successfully
                    return Status.OK;
                }

                // Should return: internal failure, activated, already activated, no matching account (assume that the thingy is already checked as valid)
            } catch (MySqlException e)
            {
                return Status.DATABASE_FAILURE;
            }


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
