using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using ArtPatio.Models;

namespace ArtPatio.Repositories
{
    public class ArtworkRepository
    {
        private readonly string _connectionString;

        public ArtworkRepository(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("DefaultConnection");
        }

        public void AddArtwork(Artwork artwork)
        {
            using (SqlConnection conn = new SqlConnection(_connectionString))
            {
                conn.Open();
                string query = "INSERT INTO Artworks (ArtName, ArtMaterial, ArtDetails, ArtImage, UserId, UserName, Price) OUTPUT INSERTED.ArtId VALUES (@ArtName, @ArtMaterial, @ArtDetails, @ArtImage, @UserId, @UserName, @Price)";

                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@ArtName", artwork.ArtName);
                    cmd.Parameters.AddWithValue("@ArtMaterial", artwork.ArtMaterial);
                    cmd.Parameters.AddWithValue("@ArtDetails", artwork.ArtDetails);
                    cmd.Parameters.AddWithValue("@ArtImage", artwork.ArtImage);
                    cmd.Parameters.AddWithValue("@UserId", artwork.UserId);
                    cmd.Parameters.AddWithValue("@UserName", artwork.UserName);
                    cmd.Parameters.AddWithValue("@Price", artwork.Price);

                    artwork.ArtId = (int)cmd.ExecuteScalar(); 
                }
            }
        }

       

        public List<Artwork> GetAllArtworks()
        {
            List<Artwork> artworks = new List<Artwork>();

            using (SqlConnection conn = new SqlConnection(_connectionString))
            {
                conn.Open();
                string query = "SELECT ArtId, ArtName, ArtMaterial, ArtDetails, ArtImage, UserId, UserName, Price, Status FROM Artworks";

                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            Artwork artwork = new Artwork
                            {
                                ArtId = reader.GetInt32(0), // ArtId
                                ArtName = reader.GetString(1), // ArtName
                                ArtMaterial = reader.GetString(2), // ArtMaterial
                                ArtDetails = reader.GetString(3), // ArtDetails
                                ArtImage = reader.GetString(4), // ArtImage
                                UserId = reader.GetInt32(5), // UserId
                                UserName = reader.GetString(6), // UserName
                                Price = reader.GetDecimal(7),
                                Status = reader.GetString(8)
                            };
                            artworks.Add(artwork);
                        }
                    }
                }
            }

            return artworks;
        }

        public List<Artwork> GetAllArtworksByUserId(int userId)
        {
            List<Artwork> artworks = new List<Artwork>();

            using (SqlConnection conn = new SqlConnection(_connectionString))
            {
                conn.Open();
                string query = "SELECT ArtId, ArtName, ArtMaterial, ArtDetails, ArtImage, Price, Status FROM Artworks WHERE UserId = @UserId";

                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@UserId", userId);

                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            Artwork artwork = new Artwork
                            {
                                ArtId = reader.GetInt32(0),
                                ArtName = reader.GetString(1),
                                ArtMaterial = reader.GetString(2),
                                ArtDetails = reader.GetString(3),
                                ArtImage = reader.GetString(4),
                                Price= reader.GetDecimal(5),
                                Status = reader.GetString(6),
                            };
                            artworks.Add(artwork);
                        }
                    }
                }
            }
            return artworks;
        }

        public List<Artwork> GetPurchasedArtworksByUserId(int userId)
        {
            var purchasedArtworks = new List<Artwork>();
            string query = "SELECT ArtId, ArtName, ArtMaterial, ArtDetails, ArtImage, Price, Status FROM Artworks WHERE Buyer = @UserId"; 

            using (var connection = new SqlConnection(_connectionString))
            {
                using (var command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@UserId", userId);
                    connection.Open();
                    using (var reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            var artwork = new Artwork
                            {
                                ArtId = reader.GetInt32(reader.GetOrdinal("ArtId")),
                                ArtName = reader.GetString(reader.GetOrdinal("ArtName")),
                                ArtMaterial = reader.GetString(reader.GetOrdinal("ArtMaterial")),
                                Price = reader.GetDecimal(reader.GetOrdinal("Price")),
                                ArtImage = reader.GetString(reader.GetOrdinal("ArtImage")),
                                Status = reader.GetString(reader.GetOrdinal("Status"))
                            };
                            purchasedArtworks.Add(artwork);
                        }
                    }
                }
            }

            return purchasedArtworks;
        }

        public Artwork GetArtworkById(int id)
        {
            Artwork artwork = null;

            using (SqlConnection conn = new SqlConnection(_connectionString))
            {
                conn.Open();
                string query = "SELECT ArtId, ArtName, ArtMaterial, ArtDetails, ArtImage, UserId, UserName, Price, Status FROM Artworks WHERE ArtId = @ArtId";

                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@ArtId", id);

                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            artwork = new Artwork
                            {
                                ArtId = reader.GetInt32(0),     // ArtId
                                ArtName = reader.GetString(1),   // ArtName
                                ArtMaterial = reader.GetString(2), // ArtMaterial
                                ArtDetails = reader.GetString(3),  // ArtDetails
                                ArtImage = reader.GetString(4),   // ArtImage
                                UserId = reader.GetInt32(5),      // UserId
                                UserName = reader.GetString(6),   // UserName
                                Price = reader.GetDecimal(7),      // Price
                                Status= reader.GetString(8)
                            };
                        }
                    }
                }
            }

            return artwork;
        }

        // New method to update the Buyer and Status columns when an artwork is purchased
        public bool BuyArtwork(int artId, int customerId)
        {
            using (SqlConnection conn = new SqlConnection(_connectionString))
            {
                conn.Open();
                string query = @"
                    UPDATE Artworks
                    SET Buyer = @CustomerId, Status = 'Sold'
                    WHERE ArtId = @ArtId AND Status = 'Available'";

                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@CustomerId", customerId); // Set the customer ID
                    cmd.Parameters.AddWithValue("@ArtId", artId); // Set the art ID

                    int rowsAffected = cmd.ExecuteNonQuery();
                    return rowsAffected > 0; // Returns true if the update was successful
                }
            }
        }




    }
}
