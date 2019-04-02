using ConnectedLevelTest.DataAccess;
using ConnectedLevelTest.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConnectedLevelTest
{
    class Program
    {
        static void Main(string[] args)
        {
            var services = new UsersTableDataService();
            services.Add(new User
            {
                Login = "user",
                Password = "123"
            });
            services.Add(new User
            {
                Login = "admin",
                Password = "root"
            });

            foreach(var user in services.GetAll())
            {
                Console.WriteLine($"{user.Id}, {user.Login}, {user.Password}");
            }
            Console.ReadLine();
        }
    }
}
