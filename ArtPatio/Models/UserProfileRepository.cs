using ArtPatio.Models;
using Microsoft.Extensions.Configuration;
using System;
using System.Data;
using System.Data.SqlClient;

namespace ArtPatio.Repositories
{
    public class UserProfileRepository
    {
        private readonly string _connectionString;

        // Constructor to inject the connection string from appsettings.json
        public UserProfileRepository(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("DefaultConnection");
        }

        // Method to insert a new user profile into the database
        public int CreateUserProfile(UserProfile userProfile)
        {
            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                try
                {
                    connection.Open();

                    string query = @"INSERT INTO UserProfile (Name, Email, Password, ConfirmPassword, Address, Contact, UserType, Description, Balance)
                             VALUES (@Name, @Email, @Password, @ConfirmPassword, @Address, @Contact, @UserType, @Description, @Balance);
                             SELECT SCOPE_IDENTITY();"; // Returns the newly created User ID

                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@Name", userProfile.Name);
                        command.Parameters.AddWithValue("@Email", userProfile.Email);
                        command.Parameters.AddWithValue("@Password", userProfile.Password);
                        command.Parameters.AddWithValue("@ConfirmPassword", userProfile.ConfirmPassword);
                        command.Parameters.AddWithValue("@Address", userProfile.Address);
                        command.Parameters.AddWithValue("@Contact", userProfile.Contact);
                        command.Parameters.AddWithValue("@UserType", userProfile.UserType);
                        command.Parameters.AddWithValue("@Description", userProfile.Description);
                        command.Parameters.AddWithValue("@Balance", userProfile.Balance); // Add this line

                        var result = command.ExecuteScalar();
                        return Convert.ToInt32(result); // Return the newly created User ID
                    }
                }
                catch (Exception ex)
                {
                    // Handle the exception as needed (log, rethrow, etc.)
                    throw new Exception("An error occurred while creating the user profile.", ex);
                }
                finally
                {
                    connection.Close();
                }
            }
        }


        // Method to check if an email already exists in the database
        public bool IsEmailExists(string email)
        {
            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                try
                {
                    connection.Open();

                    string query = "SELECT COUNT(*) FROM UserProfile WHERE Email = @Email";

                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@Email", email);

                        var count = (int)command.ExecuteScalar();
                        return count > 0; // Return true if email exists
                    }
                }
                catch (Exception ex)
                {
                    // Handle exception
                    throw new Exception("An error occurred while checking the email.", ex);
                }
                finally
                {
                    connection.Close();
                }
            }
        }

        // Method to get a user by email and password
        public UserProfile GetUserByEmailAndPassword(string email, string password)
        {
            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                try
                {
                    connection.Open();

                    // Query to get the user by email and password
                    string query = "SELECT Id, Name, Email, Address, Contact, UserType, Description, Balance FROM UserProfile WHERE Email = @Email AND Password = @Password";

                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@Email", email);
                        command.Parameters.AddWithValue("@Password", password);


                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                // Create and return UserProfile object
                                return new UserProfile
                                {
                                    Id = reader.GetInt32(0),
                                    Name = reader.GetString(1),
                                    Email = reader.GetString(2),
                                    Address = reader.GetString(3),
                                    Contact = reader.GetString(4),
                                    UserType = reader.GetString(5),
                                    Description = reader.GetString(6),
                                    Balance = reader.GetDecimal(7)
                                    // Password is typically not returned for security reasons
                                };
                            }
                            return null; // Return null if user not found
                        }
                    }
                }
                catch (Exception ex)
                {
                    // Handle exception
                    throw new Exception("An error occurred while retrieving the user.", ex);
                }
                finally
                {
                    connection.Close();
                }
            }
        }

        


    }
}