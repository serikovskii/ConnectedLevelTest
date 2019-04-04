using ConnectedLevelTest.Models;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConnectedLevelTest.DataAccess
{
    public class UsersTableDataService
    {
        private readonly string _connectionString;

        public UsersTableDataService()
        {
            _connectionString = @"Data Source=(LocalDB)\MSSQLLocalDB;AttachDbFilename=C:\Users\МукушевА.CORP\source\repos\ConnectedLevelTest\ConnectedLevelTest.DataAccess\Database.mdf;Integrated Security=True";
        }
        public List<User> GetAll()
        {
            var data = new List<User>();
            using (var connection = new SqlConnection(_connectionString))
            using (var command = connection.CreateCommand())
            {
                try
                {
                    connection.Open();
                    command.CommandText = "select * from Users";

                    var dataReader = command.ExecuteReader();
                    while (dataReader.Read())
                    {
                        int id = (int)dataReader["Id"];
                        string login = dataReader["Login"].ToString();
                        string password = dataReader["Password"].ToString();

                        data.Add(new User
                        {
                            Id = id,
                            Login = login,
                            Password = password
                        });
                    }
                    dataReader.Close();
                }
                catch (SqlException exeptions)
                {
                    // TODO obrabotka
                    throw;
                }
                catch (Exception exeptions)
                {
                    // TODO obrabotka
                    throw;
                }

            }
            return data;

        }
        public void Add(User user)
        {
            using (var connection = new SqlConnection(_connectionString))
            using (var command = connection.CreateCommand())
            {
                SqlTransaction transaction = null;
                try
                {
                    connection.Open();
                    transaction = connection.BeginTransaction();
                    command.Transaction = transaction;
                    command.CommandText = $"insert into Users values (@login, @password)";

                    SqlParameter loginParameter = new SqlParameter();
                    loginParameter.ParameterName = "@login";
                    loginParameter.SqlDbType = System.Data.SqlDbType.NVarChar;
                    loginParameter.SqlValue = user.Login;

                    SqlParameter passwordParameter = new SqlParameter
                    {
                        ParameterName = "@password",
                        SqlDbType = System.Data.SqlDbType.NVarChar,
                        SqlValue = user.Password
                    };

                    command.Parameters.AddRange(new SqlParameter[] { loginParameter, passwordParameter });

                    var affectedRows = command.ExecuteNonQuery();

                    if (affectedRows < 1) throw new Exception("Вставка не удалась");

                    transaction.Commit();
                }
                catch (SqlException exeptions)
                {
                    transaction?.Rollback();
                    // TODO obrabotka
                    throw;
                }
                catch (Exception exeptions)
                {
                    transaction?.Rollback();
                    // TODO obrabotka
                    throw;
                }

            }
        }

        public void DeleteByID(int id)
        {


        }

        public void Insert(User user)
        {


        }

       
    }
}
