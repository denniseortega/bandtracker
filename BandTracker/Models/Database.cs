using System;
using MySql.Data.MySqlClient;
using BandTrackerApp.Models;

namespace BandTrackerApp.Models
{
    public class DB
    {
        public static MySqlConnection Connection()
        {
            MySqlConnection conn = new MySqlConnection(DBConfiguration.ConnectionString);
            return conn;
        }
    }
}
