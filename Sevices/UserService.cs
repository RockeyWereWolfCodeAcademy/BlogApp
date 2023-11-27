using BlogsApp.Helpers;
using BlogsApp.Models;
using System.Security.Cryptography;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using System.Data.SqlClient;

namespace BlogsApp.Sevices
{
    public static class UserService
    {
        public static int Register(User data)
        {
            if (SqlHelper.GetDatas($"SELECT * FROM Users WHERE Username = '{data.Username}'").Rows.Count != 0)
            {
                Console.WriteLine("User with this username is already exists, choose another one!");
                return -1;
            }
            Console.WriteLine("Registration is completed");
            return SqlHelper.Exec($"INSERT INTO Users VALUES ('{data.Username}', 'N'{data.Name}', N'{data.Surname}', N'{HashPassword(data.Password)}')");
        }

        public static bool Login(string username, string password)
        {
            DataTable dataTable = SqlHelper.GetDatas($"SELECT * FROM Users WHERE Username = '{username}'");
            bool isCompleted = false;
            foreach (DataRow row in dataTable.Rows)
            {
                isCompleted = VerifyHashedPassword((string)row["Password"], password);
            }
            return isCompleted;
        }

        public static void UpdateNameOfUser(string username, string newName)
        {
            // Implement the logic to update the user in the database
            // You may use parameterized queries or an ORM for better security
            string query = $"UPDATE Users SET Name = N'{newName}' WHERE Username = '{username}'";
            SqlHelper.Exec(query);
        }

        public static void UpdateSurnameOfUser(string username, string newSurname)
        {
            // Implement the logic to update the user in the database
            // You may use parameterized queries or an ORM for better security
            string query = $"UPDATE Users SET Surname = N'{newSurname}' WHERE Username = '{username}'";
            SqlHelper.Exec(query);
        }

        public static void UpdatePasswordOfUser(string username, string newPass)
        {
            // Implement the logic to update the user in the database
            // You may use parameterized queries or an ORM for better security
            string query = $"UPDATE Users SET Password = '{HashPassword(newPass)}' WHERE Username = '{username}'";
            SqlHelper.Exec(query);
        }

        public static void DeleteUser(string username)
        {
            string query = $"DELETE FROM Users WHERE Username = '{username}'";
            SqlHelper.Exec(query);
        }

        static string HashPassword(string password)
        {
            byte[] saltBuffer;
            byte[] hashBuffer;

            using (var keyDerivation = new Rfc2898DeriveBytes(password, 16, 8192, HashAlgorithmName.SHA256))
            {
                saltBuffer = keyDerivation.Salt;
                hashBuffer = keyDerivation.GetBytes(16);
            }

            byte[] result = new byte[16 + 16];
            Buffer.BlockCopy(hashBuffer, 0, result, 0, 16);
            Buffer.BlockCopy(saltBuffer, 0, result, 16, 16);
            return Convert.ToBase64String(result);
        }
        static bool VerifyHashedPassword(string hashedPassword, string providedPassword)
        {
            byte[] hashedPasswordBytes = Convert.FromBase64String(hashedPassword);
            if (hashedPasswordBytes.Length != 16 + 16)
            {
                return false;
            }

            byte[] hashBytes = new byte[16];
            Buffer.BlockCopy(hashedPasswordBytes, 0, hashBytes, 0, 16);
            byte[] saltBytes = new byte[16];
            Buffer.BlockCopy(hashedPasswordBytes, 16, saltBytes, 0, 16);

            byte[] providedHashBytes;
            using (var keyDerivation = new Rfc2898DeriveBytes(providedPassword, saltBytes, 8192, HashAlgorithmName.SHA256))
            {
                providedHashBytes = keyDerivation.GetBytes(16);
            }

            return hashBytes.SequenceEqual(providedHashBytes);
        }
    }
}
