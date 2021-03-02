using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;

namespace brimark_backend.Utils.Database
{

    public static class DBConnection
    {
        private static MySqlConnection connection;

        static DBConnection()
        {
            // This is never called
            System.Diagnostics.Debug.WriteLine("Initialising Database Connection");
            Dictionary<string, string> databaseConnectionDetails = Data.GetDatabase();

            string connectionString = String.Format(
                "SERVER={0}; DATABASE= {1}; UID={2}; PASSWORD={3};",
                databaseConnectionDetails["ip"],
                databaseConnectionDetails["database"],
                databaseConnectionDetails["username"],
                databaseConnectionDetails["password"]
                );

            connection = new MySqlConnection(connectionString);

            try
            {
                connection.Open();
                POST.SetConnection(connection);
                System.Diagnostics.Debug.WriteLine("Database Connection Established");


            } catch (MySqlException)
            {
                System.Diagnostics.Debug.WriteLine("[FATAL] Database Connection Failed");
                // Not very good, database is unreachable
            }

        }
        private static MySqlConnection GetConnection()
        {
            return connection;
        }

        public static void X()
        {

        }

    }
}
