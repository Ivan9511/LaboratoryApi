using Microsoft.Data.Sqlite;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WpfApp
{
    public class dbConnector
    {
        public static SqliteConnection OpenConnection()
        {
            SqliteConnection connection = new SqliteConnection("Data Source=C:\\C# project\\2024\\Attempt 1\\Day3\\laboratory.db");
            connection.Open();
            return connection;
        }
    }
}
