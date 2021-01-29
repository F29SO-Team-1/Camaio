using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MySql.Data.MySqlClient;
using MySqlConnector;

namespace Login
{
    public class AppDb : IDisposable
    {
        //get the connection
        public MySqlConnection Connection { get; }

        //open connection
        public AppDb(string connectionString)
        {
            Connection = new MySqlConnection(connectionString);
        }

        //close connection
        public void Dispose()
        {
            Connection.Dispose();
        }
    }
}
