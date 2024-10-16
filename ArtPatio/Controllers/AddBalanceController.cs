using Microsoft.AspNetCore.Mvc;
using System.Data.SqlClient;
using ArtPatio.Models; // Ensure you have the Transaction model
using ArtPatio.Repositories; // Ensure you have a reference to the Transaction repository

namespace ArtPatio.Controllers
{
    public class AddBalanceController : BaseController
    {
        private readonly string _connectionString;
        private readonly TransactionRepository _transactionRepository; // Reference to the Transaction repository

        public AddBalanceController(IConfiguration configuration, TransactionRepository transactionRepository)
        {
            _connectionString = configuration.GetConnectionString("DefaultConnection"); // Get connection string from appsettings.json
            _transactionRepository = transactionRepository; // Initialize the transaction repository
        }

        // GET: /AddBalance
        public IActionResult Index()
        {
            if (HttpContext.Session.GetString("UserType") == "Customer")
            {
                // Get the current user's balance from the database
                int userId = HttpContext.Session.GetInt32("Id").Value;
                int currentBalance = GetUserBalance(userId);
                ViewBag.CurrentBalance = currentBalance; // Set the current balance to the ViewBag
                return View();
            }
            return RedirectToAction("Index", HttpContext.Session.GetString("UserType"));
        }

        [HttpPost]
        public IActionResult Add(int amount)
        {
            int userId = HttpContext.Session.GetInt32("Id").Value;

            // Update the balance in the database
            AddUserBalance(userId, amount);
            int updatedBalance = GetUserBalance(userId);
            HttpContext.Session.SetString("Balance", updatedBalance.ToString());

            // Log the balance addition as a transaction
            LogTransaction(userId, amount, updatedBalance);

            // Redirect to the index to show updated balance
            return RedirectToAction("Index");
        }

        // Method to get the user balance
        private int GetUserBalance(int userId)
        {
            int balance = 0;

            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                connection.Open();
                string query = "SELECT Balance FROM UserProfile WHERE Id = @UserId";

                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@UserId", userId);
                    // Execute the command and get the result
                    object result = command.ExecuteScalar();

                    if (result != null && result != DBNull.Value)
                    {
                        // Convert the result to decimal and then to int
                        balance = Convert.ToInt32((decimal)result);
                    }
                    else
                    {
                        balance = 0; // or any default value you want
                    }
                }
            }

            return balance;
        }

        // Method to add amount to user balance
        private void AddUserBalance(int userId, int amount)
        {
            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                connection.Open();
                string query = "UPDATE UserProfile SET Balance = Balance + @Amount WHERE Id = @UserId";

                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@Amount", amount);
                    command.Parameters.AddWithValue("@UserId", userId);
                    command.ExecuteNonQuery();
                }
            }
        }

        // Method to log the transaction
        private void LogTransaction(int userId, int amount, int updatedBalance)
        {
            var transaction = new Transaction
            {
                UserId = userId,
                TransactionType = "BalanceAdded",
                PreviousBalance = updatedBalance - amount, // The balance before adding the amount
                UpdatedBalance = updatedBalance, // The new balance after adding the amount
                TransactionDate = DateTime.Now
            };

            _transactionRepository.AddTransaction(transaction); // Save the transaction to the database
        }
    }
}
