using Microsoft.AspNetCore.Mvc;
using Backend.Models;
using System.Collections.Specialized;
using System.Data.SqlClient;
using System.Security.Cryptography;
using Microsoft.AspNetCore.Cryptography.KeyDerivation;
using configNamespace = System.Configuration;

namespace Backend.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UserController : ControllerBase
    {
        // Return connection string
        private static string GetConnectionString()
        {
            var conn = configNamespace.ConfigurationManager.AppSettings["conn"];
            if (conn is not null)
            {
                return conn.ToString();
            }
            return "";
        }

        // Create New User Query
        private static string CreateNewUserQuery(User user)
        {
            return $"INSERT INTO Users (first, last, company, email, password) VALUES ('{user.first}', '{user.last}', '{user.company}', '{user.email}', '{user.password}')";
        }

        // Get User By Email
        private static string GetUserByEmailQuery(string email)
        {
            return $"SELECT * FROM Users WHERE email = '{email}'";
        }

        // Returns the hashed value of a given password
        public static string Hash(string password)
        {
            // Generate 128-bit salt
            byte[] salt = new byte[128 / 8];
            var rngCsp = RandomNumberGenerator.GetBytes;

            // Derive a 256-bit subkey
            string hashed = Convert.ToBase64String(KeyDerivation.Pbkdf2(
                password: password,
                salt: salt,
                prf: KeyDerivationPrf.HMACSHA256,
                iterationCount: 100000,
                numBytesRequested: 256 / 8));

            return hashed;
        }

        // Post method to insert a new User
        // into the database
        [HttpPost]
        public string PostUser(User user)
        {
            // Hash Incoming Password
            user.password = Hash(user.password);
            string connString = GetConnectionString();
            using (SqlConnection connection = new SqlConnection(connString))
            {
                SqlCommand command = new SqlCommand(CreateNewUserQuery(user), connection);
                command.Connection.Open();
                command.ExecuteNonQuery();
                connection.Close();
            }
            return "User successfully created.";
        }

        // Get method used to retrieve a user
        // in the database by email
        [HttpGet("{email}")]
        public User Get(string email)
        {
            User user = new User();
            string connString = GetConnectionString();
            using (SqlConnection connection = new SqlConnection(connString))
            {
                SqlCommand command = new SqlCommand(GetUserByEmailQuery(email), connection);
                command.Connection.Open();
                using (SqlDataReader reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        user.id = reader.GetInt32(0);
                        user.first = reader.GetString(1);
                        user.last = reader.GetString(2);
                        user.company = reader.GetString(3);
                        user.email = reader.GetString(4);
                        user.password = reader.GetString(5);
                    }
                }
                connection.Close();
            }
            return user;
        }
    }
}