using System;
using System.Collections.Generic;
using System.Data.SQLite;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace UserAuthenticationApp
{
    public class DataHandler
    {
        private readonly DataHandler handler;

        // Konstruktor třídy DataHandler
        public DataHandler()
        {
            handler = this;
        }

        public string GetPasswordHash(string password)
        {
            using (SHA256 sha256 = SHA256.Create())
            {
                byte[] bytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(password));
                StringBuilder builder = new StringBuilder();
                for (int i = 0; i < bytes.Length; i++)
                {
                    builder.Append(bytes[i].ToString("x2"));
                }
                return builder.ToString();
            }
        }

        private string connectionString = "Data Source=mydatabase.db;Version=3;";

        public void SaveUser(User user)
        {
            using (SQLiteConnection connection = new SQLiteConnection(connectionString))
            {
                connection.Open();
                using (SQLiteCommand cmd = new SQLiteCommand(connection))
                {
                    cmd.CommandText = "INSERT INTO Users (Username, PasswordHash) VALUES (@username, @passwordHash)";
                    cmd.Parameters.AddWithValue("@username", user.Username);
                    cmd.Parameters.AddWithValue("@passwordHash", user.PasswordHash);
                    cmd.ExecuteNonQuery();
                }
            }
        }

        public List<User> GetAllUsers()
        {
            List<User> users = new List<User>();
            using (SQLiteConnection connection = new SQLiteConnection(connectionString))
            {
                connection.Open();
                using (SQLiteCommand cmd = new SQLiteCommand("SELECT * FROM Users", connection))
                {
                    using (SQLiteDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            User user = new User
                            {
                                Username = reader["Username"].ToString(),
                                PasswordHash = reader["PasswordHash"].ToString()
                            };
                            users.Add(user);
                        }
                    }
                }
            }
            return users;
        }
    }
}
