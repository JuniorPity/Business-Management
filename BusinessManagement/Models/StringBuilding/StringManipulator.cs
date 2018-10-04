using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Web;

namespace BusinessManagement.Models.StringBuilding
{
    public class StringManipulator
    {
        // Hashing and Random Algorithsm
        #region Hashing and Random Algorithms
        /*
        * Function: GenerateSalt()
        * Purpose: Generate a random salt to help hash passwords
        * Author: Jordan Pitner 9/20/2018
        */
        public static string GenerateSalt()
        {
            Random random = new Random();
            const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";

            return new string(Enumerable.Repeat(chars, 15)
              .Select(s => s[random.Next(s.Length)]).ToArray());
        }

        /*
        * Function: CreateInviteCode(int length)
        * Purpose: Generate a random string of chars to act as the invite code
        * Author: Jordan Pitner 10/3/2018
        */
        public static string CreateInviteCode(int length)
        {
            Random random = new Random();

            // Characters to choose from
            const string chars = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789_-";

            // Generate string from random generator and length
            return new string(Enumerable.Repeat(chars, length)
                .Select(s => s[random.Next(s.Length)]).ToArray());
        }

        /*
        * Function: GenerateHashedPassword(string salt, string password)
        * Purpose: Generate a salted and hashed password for storage in the database
        * Author: Jordan Pitner 9/20/2018
        */
        public static string GenerateHashedPassword(string salt, string password)
        {
            using (SHA256 sha256 = SHA256.Create())
            {
                string hash = "OzzyOsbourneSTPJudasPriestBringMeTheHorizon" + salt + "JP" + password + "JP" + salt + "MetallicaKornGodsmackLedZeppelin";
                byte[] bytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(hash));

                // Convert byte array to a string   
                StringBuilder builder = new StringBuilder();
                for (int i = 0; i < bytes.Length; i++)
                {
                    builder.Append(bytes[i].ToString("x2"));
                }

                return builder.ToString();
            }
        }

        /*
        * Function: GenerateIdNumber(int size)
        * Purpose: Generate a random series of numbers for EmployerID numbers
        * Author: Jordan Pitner 9/20/2018
        */
        public static string GenerateIdNumber(int size)
        {
            Random random = new Random();
            const string chars = "0123456789";

            // Get random numerics from the char string
            return new string(Enumerable.Repeat(chars, size)
              .Select(s => s[random.Next(s.Length)]).ToArray());
        }
        #endregion
    }
}