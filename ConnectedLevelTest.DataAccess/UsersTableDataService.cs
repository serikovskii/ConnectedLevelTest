using ConnectedLevelTest.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Configuration;
using System.Data.Common;

namespace ConnectedLevelTest.DataAccess
{
    public class UsersTableDataService
    {
        private readonly string _connectionString;
        private readonly string _ownerName;
        private readonly DbProviderFactory _providerFactory;

        public UsersTableDataService()
        {
            _connectionString = ConfigurationManager.ConnectionStrings["mainAppCollectionString"].ConnectionString;
            _ownerName = ConfigurationManager.AppSettings["ownerNane"];
            _providerFactory = DbProviderFactories.GetFactory(ConfigurationManager
                .ConnectionStrings["mainAppCollectionString"]
                .ProviderName); 
        }
        public List<User> GetAll()
        {
            var data = new List<User>();
            using (var connection = _providerFactory.CreateConnection())
            using (var command = connection.CreateCommand())
            {
                try
                {
                    connection.ConnectionString = _connectionString;
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
                catch (DbException exeptions)
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
            using (var connection = _providerFactory.CreateConnection())
            using (var command = connection.CreateCommand())
            {
                DbTransaction transaction = null;
                try
                {
                    connection.ConnectionString = _connectionString;
                    connection.Open();
                    transaction = connection.BeginTransaction();
                    command.Transaction = transaction;
                    command.CommandText = $"insert into Users values (@login, @password)";

                    DbParameter loginParameter = command.CreateParameter();
                    loginParameter.ParameterName = "@login";
                    loginParameter.DbType = System.Data.DbType.String;
                    loginParameter.Value = user.Login;

                    DbParameter passwordParameter = command.CreateParameter();
                    loginParameter.ParameterName = "@password";
                    loginParameter.DbType = System.Data.DbType.String;
                    loginParameter.Value = user.Password;

                    command.Parameters.AddRange(new DbParameter[] { loginParameter, passwordParameter });

                    var affectedRows = command.ExecuteNonQuery();

                    if (affectedRows < 1) throw new Exception("Вставка не удалась");

                    transaction.Commit();
                    transaction.Dispose();
                }
                catch (DbException exeptions)
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
                finally
                {
                    transaction?.Dispose();
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
