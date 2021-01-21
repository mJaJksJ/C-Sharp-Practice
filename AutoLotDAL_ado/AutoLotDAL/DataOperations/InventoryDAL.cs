using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using AutoLotDAL.Models;
using System.Linq;

namespace AutoLotDAL.DataOperations
{
    /// <summary>
    /// sql-queries logic
    /// </summary>
    public class InventoryDal
    {
        private static readonly SqlConnectionStringBuilder ConnectStringBuilder = new SqlConnectionStringBuilder
        {
            InitialCatalog = "AutoLot",
            DataSource = @"DESKTOP-MFNAUQT\SQLEXPRESS",
            ConnectTimeout = 30,
            IntegratedSecurity = true
        };

        private readonly string _connectionString;
        private SqlConnection _sqlConnection;
        
        /// <summary>
        /// constructor with default connection string
        /// </summary>
        public InventoryDal() : this(ConnectStringBuilder.ConnectionString)
        {
        }

        /// <summary>
        /// constructor with user's connection string
        /// </summary>
        /// <param name="connectionString">user's connection string</param>
        public InventoryDal(string connectionString) => _connectionString = connectionString;
        
        private void OpenConnection()
        {
            _sqlConnection = new SqlConnection {ConnectionString = _connectionString};
            _sqlConnection.Open();
        }

        private void CloseConnection()
        {
            if (_sqlConnection?.State != ConnectionState.Closed)
            {
                _sqlConnection?.Close();
            }
        }

        /// <summary>
        /// return all cars from dbo.Inventory
        /// </summary>
        /// <returns>list of cars</returns>
        public List<Car> GetAllInventory()
        {
            OpenConnection();
            List<Car> inventory = new List<Car>();
            string sql = $"SELECT * FROM Inventory";
            using (SqlCommand command = new SqlCommand(sql, _sqlConnection))
            {
                command.CommandType = CommandType.Text;
                SqlDataReader dataReader = command.ExecuteReader(CommandBehavior.CloseConnection);
                while (dataReader.Read())
                {
                    inventory.Add(new Car
                    {
                        CarId = (int) dataReader["CarId"],
                        Color = (string) dataReader["Color"],
                        Make = (string) dataReader["Make"],
                        PetName = (string) dataReader["PetName"]
                    });
                }
                dataReader.Close();
            }
            return inventory;
        }

        /// <summary>
        /// get car by id from dbo.Inventory
        /// </summary>
        /// <param name="id">car id</param>
        /// <returns>car object</returns>
        public Car GetCar(int id)
        {
            OpenConnection();
            Car car = null;
            string sql = $"SELECT * FROM Inventory WHERE CarId = @id";
            using (SqlCommand command = new SqlCommand(sql, _sqlConnection))
            {
                command.Parameters.Add(new SqlParameter("@id", id));
                SqlDataReader dataReader = command.ExecuteReader(CommandBehavior.CloseConnection);
                while (dataReader.Read())
                {
                    car = new Car
                    {
                        CarId = (int) dataReader["CarId"],
                        Color = (string) dataReader["Color"],
                        Make = (string) dataReader["Make"],
                        PetName = (string) dataReader["PetName"]
                    };
                    
                }
                dataReader.Close();
            }

            return car;
        }

        /// <summary>
        /// add new car to dbo.Inventory
        /// </summary>
        /// <param name="color">color of car</param>
        /// <param name="make">model of car</param>
        /// <param name="petName">pet name of car</param>
        public void InsertAuto(string color, string make, string petName)
        {
            int id = GetAllInventory().OrderBy(x => x.CarId).Select(x => x.CarId).Last() + 1;
            OpenConnection();
            string sql = $"Insert Into Inventory (CarId, Make, Color, PetName) Values ({id}, @Make, @Color, @PetName)";

            using (SqlCommand command = new SqlCommand(sql, _sqlConnection))
            {
                command.Parameters.Add(new SqlParameter("@Make", make));
                command.Parameters.Add(new SqlParameter("@Color", color));
                command.Parameters.Add(new SqlParameter("@PetName", petName));
                command.ExecuteNonQuery();
            }

            CloseConnection();
        }

        /// <summary>
        /// add new car to dbo.Inventory
        /// </summary>
        /// <param name="car">adding car object</param>
        public void InsertAuto(Car car)
        {
            InsertAuto(car.Make, car.Color, car.PetName);
        }

        /// <summary>
        /// delete car from dbo.Inventory
        /// </summary>
        /// <param name="id">if od deleting car</param>
        /// <exception cref="Exception">throw when triggered SqlException</exception>
        public void DeleteCar(int id)
        {
            OpenConnection();
            string sql = $"DELETE FROM Inventory WHERE CarId = @id";
            using (SqlCommand command = new SqlCommand(sql, _sqlConnection))
            {
                try
                {
                    command.Parameters.Add(new SqlParameter("@id", id));
                    command.ExecuteNonQuery();
                }
                catch (SqlException ex)
                {
                    throw new Exception("Sorry! That car is on order!", ex);
                }
            }
            CloseConnection();
        }

        /// <summary>
        /// delete car in dbo.Inventory
        /// </summary>
        /// <param name="id">id of updating car</param>
        /// <param name="newPetName">new pet name for car</param>
        /// <exception cref="Exception">throw when triggered SqlException</exception>
        public void UpdateCarPetName(int id, string newPetName)
        {
            OpenConnection();
            string sql = $"UPDATE Inventory SET PetName = @newPetName WHERE CarId = @id";
            using (SqlCommand command = new SqlCommand(sql, _sqlConnection))
            {
                try
                {
                    command.Parameters.Add(new SqlParameter("@newPetName", newPetName));
                    command.Parameters.Add(new SqlParameter("@id", id));
                    command.ExecuteNonQuery();
                }
                catch (SqlException ex)
                {
                    throw new Exception("Sorry! That car is on order!", ex);
                }
            }
            CloseConnection();
        }
        
        /// <summary>
        /// check pet name of car
        /// </summary>
        /// <param name="carId">id of checking car</param>
        /// <returns>car's pet name</returns>
        public string LookUpPetName(int carId)
        {
            OpenConnection();
            string carPetName;
            using (SqlCommand command = new SqlCommand("Get_PetName", _sqlConnection) )
            {
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.Add(new SqlParameter
                {
                    ParameterName = "@carId",
                    SqlDbType = SqlDbType.Int,
                    Value = carId,
                    Direction = ParameterDirection.Input
                });
                command.Parameters.Add(new SqlParameter
                {
                    ParameterName = "@petName",
                    SqlDbType = SqlDbType.Char,
                    Size = 10,
                    Direction = ParameterDirection.Output
                });
                command.ExecuteNonQuery();
                carPetName = command.Parameters["@petName"].Value.ToString();
                CloseConnection();
            }
            return carPetName;
        }

        /// <summary>
        /// transact customer to credit risc
        /// </summary>
        /// <param name="throwEx">id be failing emulation</param>
        /// <param name="customerId">transacted customer</param>
        /// <exception cref="Exception">throw if throwEx==true</exception>
        public void ProcessCreditRisk(bool throwEx, int customerId)
        {
            OpenConnection();
            string firstName;
            string lastName;
            var cmdSelect = new SqlCommand($"SELECT * FROM Customers WHERE CustID = @customerId", _sqlConnection);
            cmdSelect.Parameters.Add(new SqlParameter("@customerId", customerId));
            using (var dataReader = cmdSelect.ExecuteReader())
            {
                if (dataReader.HasRows)
                {
                    dataReader.Read();
                    firstName = (string) dataReader["FirstName"];
                    lastName = (string) dataReader["LastName"];
                }
                else
                {
                    CloseConnection();
                    return;
                }
            }

            var cmdRemove = new SqlCommand($"Delete from Customers WHERE CustID = @customerId", _sqlConnection);
            cmdRemove.Parameters.Add(new SqlParameter("@customerId", customerId));
            var cmdInsert =
                new SqlCommand($"INSERT INTO CreditRisks (FirstName, LastName) VALUES(@firstName, @lastName)", _sqlConnection);
            cmdInsert.Parameters.Add(new SqlParameter("@firstName", firstName));
            cmdInsert.Parameters.Add(new SqlParameter("@lastName", lastName));
            SqlTransaction tx = null;
            try
            {
                tx = _sqlConnection.BeginTransaction();
                cmdInsert.Transaction = tx;
                cmdRemove.Transaction = tx;
                cmdInsert.ExecuteNonQuery();
                cmdRemove.ExecuteNonQuery();
                if (throwEx)
                {
                    throw new Exception("Sorry! Database error! Tx failed...");
                }

                tx.Commit();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                tx?.Rollback();
            }
            finally
            {
                CloseConnection();
            }
        }
        
    }
}