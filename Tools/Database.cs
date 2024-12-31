using MySql.Data.MySqlClient;
using System;
using System.Data;

namespace LuminaryEmu {
    internal class Database {
        public static MySqlConnection dbcon;
        public static object locker;

        public static void Initialize() {
            locker = new object();

            string server = "localhost"; // MySQL Host IP
            string database = "luminary"; // MySQL Database
            string user = "root"; // MySQL Login Username
            string password = "root"; // MySQL Login Password

            string connectionString =
            "Server=" + server + ";" +
            "Database=" + database + ";" +
            "User ID=" + user + ";" +
            "Password=" + password + ";" +
            "Pooling=false";

            dbcon = new MySqlConnection(connectionString);
            dbcon.Open();
            Tools.Log("Connected to database - " + database);
        }

        public static IDataReader Read(string query) {
            IDataReader reader;
            lock (locker) {
                IDbCommand cmd = dbcon.CreateCommand();
                cmd.CommandText = query;
                reader = cmd.ExecuteReader();
                cmd.Dispose();
                cmd = null;
            }
            return reader;
        }

        public static int Query(string query) {
            int result = 0;
            lock (locker) {
                IDbCommand cmd = dbcon.CreateCommand();
                cmd.CommandText = query;
                result = cmd.ExecuteNonQuery();
                cmd.Dispose();
                cmd = null;
            }
            return result;
        }

        public static void Close() {
            dbcon.Close();
            Console.WriteLine("Database connection closed.");
        }
    }
}
