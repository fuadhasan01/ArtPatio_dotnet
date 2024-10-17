using ArtPatio.Models;
using System.Data.SqlClient;

namespace ArtPatio.Repositories
{
    public class UserRepository
    {
        private readonly string _connectionString;

        public UserRepository(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("DefaultConnection");
        }

        public List<UserProfile> GetAllUsers()
        {
            List<UserProfile> users = new List<UserProfile>();

            using (SqlConnection conn = new SqlConnection(_connectionString))
            {
                conn.Open();
                string query = "SELECT Id, Name, Email, Address, Contact, UserType, Description, Balance FROM UserProfile";

                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            UserProfile user = new UserProfile
                            {
                                Id = reader.GetInt32(0), // UserId
                                Name = reader.GetString(1), // UserName
                                Email = reader.GetString(2), // UserEmail
                                Address = reader.GetString(3), // UserAddress
                                Contact = reader.GetString(4), // UserContact
                                UserType = reader.GetString(5), // UserType
                                Description = reader.GetString(6), // UserDescription
                                Balance = reader.GetDecimal(7)
                            };
                            users.Add(user);
                        }
                    }
                }
            }
            return users;
        }

        public UserProfile GetUserById(int id)
        {
            UserProfile user = null; // Initialize user as null

            using (SqlConnection conn = new SqlConnection(_connectionString))
            {
                conn.Open();
                string query = "SELECT Id, Name, Email, Address, Contact, UserType, Description, Balance FROM UserProfile WHERE Id = @Id";

                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@Id", id); // Add the user ID as a parameter

                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        if (reader.Read()) // Check if any record is found
                        {
                            user = new UserProfile
                            {
                                Id = reader.GetInt32(0), // UserId
                                Name = reader.GetString(1), // UserName
                                Email = reader.GetString(2), // UserEmail
                                Address = reader.GetString(3), // UserAddress
                                Contact = reader.GetString(4), // UserContact
                                UserType = reader.GetString(5), // UserType
                                Description = reader.GetString(6), // UserDescription
                                Balance = reader.GetDecimal(7)
                            };
                        }
                    }
                }
            }
            return user; // Return the found user or null if not found
        }

        // Method to get a customer's balance by their user ID
        public decimal GetCustomerBalance(int userId)
        {
            decimal balance = 0; // Default value for balance
            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                try
                {
                    connection.Open();

                    // Query to retrieve the user's balance
                    string query = "SELECT Balance FROM UserProfile WHERE Id = @UserId";

                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@UserId", userId);

                        // Execute the query and get the balance
                        var result = command.ExecuteScalar();
                        if (result != null)
                        {
                            balance = Convert.ToDecimal(result);
                        }
                    }
                }
                catch (Exception ex)
                {
                    // Handle the exception as needed (log, rethrow, etc.)
                    throw new Exception("An error occurred while retrieving the customer's balance.", ex);
                }
                finally
                {
                    connection.Close();
                }
            }

            return balance; // Return the balance (0 if not found)
        }

        public bool UpdateCustomerBalance(int customerId, decimal newBalance)
        {
            string query = "UPDATE UserProfile SET Balance = @Balance WHERE Id = @UserId"; // Adjust as necessary

            using (var connection = new SqlConnection(_connectionString))
            {
                using (var command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@Balance", newBalance);
                    command.Parameters.AddWithValue("@UserId", customerId);
                    connection.Open();
                    int rowsAffected = command.ExecuteNonQuery();
                    return rowsAffected > 0; // Returns true if the update was successful
                }
            }
        }



    }
}