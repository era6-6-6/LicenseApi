using MySql.Data.MySqlClient;
using System.Collections.Concurrent;
using System.Diagnostics;

namespace LicenseApi.Managers.Database
{
    public class DatabaseManager
    {
        readonly MySqlConnection? Conn;
        readonly MySqlCommand? Command;
        public static string? ConnectionString = "";
        
        
     

        public DatabaseManager()
        {
            Conn = new MySqlConnection(GenerateConnectionString());
            Command = Conn.CreateCommand();
            Task.Run(ExecuteQuery);
        }


       
        public static string? GenerateConnectionString()
        {
            if (ConnectionString == "")
            {
                var ConnectionStringBuilder = new MySqlConnectionStringBuilder();
                ConnectionStringBuilder.Server = "localhost";
                ConnectionStringBuilder.Port = 3306;
                ConnectionStringBuilder.UserID = "root";
                ConnectionStringBuilder.Password = "";
                ConnectionStringBuilder.Database = "KryptonMain";
                ConnectionStringBuilder.ConvertZeroDateTime = true;
                ConnectionStringBuilder.Pooling = true;
                ConnectionStringBuilder.MaximumPoolSize = 100;
                ConnectionStringBuilder.SslMode = MySqlSslMode.Prefered;
                ConnectionString = ConnectionStringBuilder.ToString();
            }

            return ConnectionString;
        }
        private async Task GetLicensedAccounts()
        {
            Command.CommandText = $"SELECT * FROM `accounts`";
            Command.Parameters.Clear();
            MySqlDataReader reader = Command.ExecuteReader();
            while (reader.Read())
            {
                var id = reader.GetInt32("id");
                var ownerId = reader.GetInt32("user_id");
                var darkorbitId = reader.GetInt32("darkorbit_user_id");
                var key = reader.GetString("license_key");
                var expire_at = reader.GetDateTime("license_time");

                if (!LicenseManager.ValidateLicense(darkorbitId))
                {
                    if (expire_at > DateTime.Now)
                    {
                        LicenseManager.Add(darkorbitId, true);
                    }
                    
                }
            }
            reader.Close();
            await Task.CompletedTask;

        }
        private async Task ExecuteQuery()
        {
            while (true)
            {

                try
                {
                    Conn?.Open();

                    if (Command != null)
                    {
                        Stopwatch s = new Stopwatch();
                        s.Start();
                        await GetLicensedAccounts();
                        Console.WriteLine("Updated: " + s.ElapsedMilliseconds);
                    }
                }
                catch (Exception ex)
                {
                    System.Console.WriteLine(ex);
                }

                await Conn?.CloseAsync()!;

                await Task.Delay(2000);
            }
            // ReSharper disable once FunctionNeverReturns
        }
    }
}
