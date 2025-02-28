using System;
using System.Collections.Generic;
using System.Security.Cryptography;
using System.Text;

namespace StudentManagementSystem
{
    class Program
    {
        // In-memory "database" of users
        private static Dictionary<string, (string PasswordHash, string Role)> users = new Dictionary<string, (string, string)>
        {
            // Predefined users for testing
            { "admin", (HashPassword("admin123"), "Admin") },
            { "student", (HashPassword("student123"), "Student") }
        };

        static void Main(string[] args)
        {
            Console.WriteLine("Welcome to Student Management System");

            // Simulate login form in console
            Console.Write("Username: ");
            string username = Console.ReadLine();
            Console.Write("Password: ");
            string password = Console.ReadLine();

            if (string.IsNullOrEmpty(username) || string.IsNullOrEmpty(password))
            {
                Console.WriteLine("Please enter both username and password.");
                return;
            }

            if (AuthenticateUser(username, password, out string role))
            {
                Console.WriteLine("Login Successful!");
                Console.WriteLine($"Logged in as: {role}");
                // Simulate navigating to the Dashboard
                ShowDashboard(role);
            }
            else
            {
                Console.WriteLine("Invalid username or password.");
            }
        }

        private static bool AuthenticateUser(string username, string password, out string role)
        {
            role = null;
            if (users.ContainsKey(username))
            {
                var user = users[username];
                string storedHash = user.PasswordHash;
                role = user.Role;
                return VerifyPassword(password, storedHash);
            }
            return false;
        }

        private static string HashPassword(string password)
        {
            using (SHA256 sha256 = SHA256.Create())
            {
                byte[] bytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(password));
                StringBuilder builder = new StringBuilder();
                foreach (byte b in bytes)
                {
                    builder.Append(b.ToString("x2"));
                }
                return builder.ToString();
            }
        }

        private static bool VerifyPassword(string enteredPassword, string storedHash)
        {
            string enteredHash = HashPassword(enteredPassword);
            return enteredHash.Equals(storedHash, StringComparison.OrdinalIgnoreCase);
        }

        private static void ShowDashboard(string role)
        {
            Console.WriteLine("\n--- Dashboard ---");
            Console.WriteLine($"Welcome, {role}!");
            Console.WriteLine("You can perform actions based on your role here.");
        }
    }
}
