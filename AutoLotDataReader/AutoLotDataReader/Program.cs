using System.Data.SqlClient;
using static System.Console;

namespace AutoLotDataReader
{
    internal class Program
    {
        static void ShowConnectionStatus(SqlConnection connection)
        {
            WriteLine("*** Info about your connection ***");
            WriteLine($"Database location: {connection.DataSource}");
            WriteLine($"Database name: {connection.Database}");
            WriteLine($"Timeout: {connection.ConnectionTimeout}");
            WriteLine($"Connection state: {connection.State}\n");
        }


        public static void Main(string[] args)
        {
            WriteLine("*** Fun with Data Readers ***\n");

            var connectStringBuilder = new SqlConnectionStringBuilder
            {
                InitialCatalog = "AutoLot",
                DataSource = @"DESKTOP-MFNAUQT\SQLEXPRESS",
                ConnectTimeout = 30,
                IntegratedSecurity = true
            };
            
            using (SqlConnection connection = new SqlConnection())
            {
                connection.ConnectionString = connectStringBuilder.ConnectionString;
                connection.Open();
                ShowConnectionStatus(connection);
                string sql = "Select * From Inventory";
                SqlCommand sqlCommand = new SqlCommand(sql, connection);
                using (SqlDataReader sqlDataReader = sqlCommand.ExecuteReader())
                {
                    while (sqlDataReader.Read())
                    {
                        WriteLine($"-> CarId: {sqlDataReader["CarId"]},\t" +
                                  $"Make: {sqlDataReader["Make"]},\t" +
                                  $"PetName: {sqlDataReader["PetName"]},\t" +
                                  $"Color: {sqlDataReader["Color"]}.");
                    }
                }
            }

            ReadLine();
        }
    }
}