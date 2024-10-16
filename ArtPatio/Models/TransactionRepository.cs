using ArtPatio.Models;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;

namespace ArtPatio.Repositories
{
    public class TransactionRepository
    {
        private readonly string _connectionString;

        public TransactionRepository(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("DefaultConnection");
        }

        // Add a new transaction to the database
        public void AddTransaction(Transaction transaction)
        {
            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                connection.Open();

                string query = @"INSERT INTO Transactions (UserId, ArtId, TransactionType, PreviousBalance, UpdatedBalance, TransactionDate, BuyerId, ArtistId)
                                 VALUES (@UserId, @ArtId, @TransactionType, @PreviousBalance, @UpdatedBalance, @TransactionDate, @BuyerId, @ArtistId)";

                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@UserId", transaction.UserId);
                    command.Parameters.AddWithValue("@ArtId", (object)transaction.ArtId ?? DBNull.Value);
                    command.Parameters.AddWithValue("@TransactionType", transaction.TransactionType);
                    command.Parameters.AddWithValue("@PreviousBalance", transaction.PreviousBalance);
                    command.Parameters.AddWithValue("@UpdatedBalance", transaction.UpdatedBalance);
                    command.Parameters.AddWithValue("@TransactionDate", transaction.TransactionDate);
                    command.Parameters.AddWithValue("@BuyerId", (object)transaction.BuyerId ?? DBNull.Value);
                    command.Parameters.AddWithValue("@ArtistId", (object)transaction.ArtistId ?? DBNull.Value);

                    command.ExecuteNonQuery();
                }
            }
        }

        // Get transaction history by user ID
        public List<Transaction> GetTransactionHistory(int userId)
        {
            List<Transaction> transactions = new List<Transaction>();

            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                connection.Open();

                string query = "SELECT * FROM Transactions WHERE UserId = @UserId ORDER BY TransactionDate DESC";

                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@UserId", userId);

                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            transactions.Add(new Transaction
                            {
                                TransactionId = reader.GetInt32(0),
                                UserId = reader.GetInt32(1),
                                ArtId = reader.IsDBNull(2) ? (int?)null : reader.GetInt32(2),
                                TransactionType = reader.GetString(3),
                                PreviousBalance = reader.GetDecimal(4),
                                UpdatedBalance = reader.GetDecimal(5),
                                TransactionDate = reader.GetDateTime(6),
                                BuyerId = reader.IsDBNull(7) ? (int?)null : reader.GetInt32(7),
                                ArtistId = reader.IsDBNull(8) ? (int?)null : reader.GetInt32(8)
                            });
                        }
                    }
                }
            }

            return transactions;
        }
    }
}
