using ArtPatio.Models;
using ArtPatio.Repositories;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.IO;

namespace ArtPatio.Controllers
{
    public class ArtController : BaseController
    {
        private readonly ArtworkRepository _artworkRepository;
        private readonly UserRepository _userRepository;
        private readonly TransactionRepository _transactionRepository; 
        //

        public ArtController(ArtworkRepository artworkRepository, UserRepository userRepository, TransactionRepository transactionRepository)
        {
            _artworkRepository = artworkRepository;
            _userRepository = userRepository;
            _transactionRepository = transactionRepository;
        }


        // GET: /Art/Index
        public IActionResult Index(string searchTerm, decimal? minPrice, decimal? maxPrice, string artworkStatus)
        {
            List<Artwork> artworks = _artworkRepository.GetAllArtworks();

            // If a search term is provided, filter the artworks
            if (!string.IsNullOrEmpty(searchTerm))
            {
                artworks = artworks.Where(a => a.ArtName.Contains(searchTerm, StringComparison.OrdinalIgnoreCase)).ToList();
            }
            if (minPrice.HasValue)
            {
                artworks = artworks.Where(a => a.Price >= minPrice.Value).ToList();
            }

            if (maxPrice.HasValue)
            {
                artworks = artworks.Where(a => a.Price <= maxPrice.Value).ToList();
            }
            if (!string.IsNullOrEmpty(artworkStatus))
            {
                artworks = artworks.Where(a => a.Status == artworkStatus).ToList();
            }

            return View(artworks);
        }

        // GET: /Art/Upload
        // GET: /Art/Upload
        public IActionResult Upload()
        {
            if (HttpContext.Session.GetInt32("Id") != null)
            {
                if (HttpContext.Session.GetString("UserType") == "Artist")
                {
                    // Get the artist's ID from the session
                    int artistId = HttpContext.Session.GetInt32("Id").GetValueOrDefault();

                   
                    List<Artwork> artistArtworks = _artworkRepository.GetAllArtworksByUserId(artistId);

                    
                    ViewBag.ArtistArtworks = artistArtworks;

                    return View();
                }
                else
                {
                    return NoContent();
                }
            }
            return RedirectToAction("Login", "Account");
        }


        [HttpPost]
        public IActionResult Upload(Artwork artwork, IFormFile artImage)
        {
            if (artImage != null && artImage.Length > 0)
            {
                // Save the image to a directory and get the path
                var filePath = Path.Combine("wwwroot/images/artworks", artImage.FileName);
                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    artImage.CopyTo(stream);
                }

                artwork.ArtImage = $"/images/artworks/{artImage.FileName}"; // Save the relative path
            }

            // Get the user ID from the session
            artwork.UserId = HttpContext.Session.GetInt32("Id").GetValueOrDefault();
            artwork.UserName = HttpContext.Session.GetString("Name");

            // Add the artwork to the database
            _artworkRepository.AddArtwork(artwork);
            TempData["SuccessMessage"] = "Artwork uploaded successfully!";
            return RedirectToAction("Upload", "Art"); 
        }

        public IActionResult Details(int id)
        {
            var artwork = _artworkRepository.GetArtworkById(id);
            if (artwork == null)
            {
                return NotFound();
            }
            return View(artwork);
        }

        [HttpPost]
        public IActionResult Buy(int artId)
        {
            // Check if the user is logged in
            if (HttpContext.Session.GetInt32("Id") == null)
            {
                // If not logged in, redirect to the login page
                return RedirectToAction("Login", "Account");
            }

            // Get the customer ID (buyer) from the session
            int userId = HttpContext.Session.GetInt32("Id").GetValueOrDefault();
            string userName = HttpContext.Session.GetString("Name");

            // Retrieve the artwork by its ID
            Artwork artwork = _artworkRepository.GetArtworkById(artId);

            // Check if the artwork is available for purchase
            if (artwork == null || artwork.Status != "Available")
            {
                // If the artwork is not available, display an error message
                TempData["ErrorMessage"] = "Artwork is no longer available for purchase.";
                return RedirectToAction("Details", "Art", new { id = artId });
            }

            // Retrieve the customer's balance
            decimal userBalance = _userRepository.GetCustomerBalance(userId);

            // Check if the customer's balance is sufficient
            if (userBalance < artwork.Price)
            {
                // If the balance is insufficient, display an error message
                TempData["ErrorMessage"] = "You have insufficient balance to purchase this artwork.";
                return RedirectToAction("Details", "Art", new { id = artId });
            }

            // Update the artwork: set the buyer and change the status to 'Sold'
            bool purchaseSuccessful = _artworkRepository.BuyArtwork(artId, userId);

            if (purchaseSuccessful)
            {
                // Deduct the artwork price from the customer's balance
                bool userBalanceUpdated = _userRepository.UpdateCustomerBalance(userId, userBalance - artwork.Price);

                // Update the artist's balance by adding the artwork price
                bool artistBalanceUpdated = _userRepository.UpdateCustomerBalance(artwork.UserId, _userRepository.GetCustomerBalance(artwork.UserId) + artwork.Price);

                // Transaction Logging
                var userTransaction = new Transaction
                {
                    UserId = userId,
                    TransactionType = "ArtworkPurchased",
                    ArtId = artId,
                    PreviousBalance = userBalance,
                    UpdatedBalance = userBalance - artwork.Price,
                    TransactionDate = DateTime.Now,
                    ArtistId = artwork.UserId
                };

                var artistTransaction = new Transaction
                {
                    UserId = artwork.UserId,
                    TransactionType = "ArtworkSold",
                    ArtId = artId,
                    PreviousBalance = _userRepository.GetCustomerBalance(artwork.UserId) - artwork.Price,
                    UpdatedBalance = _userRepository.GetCustomerBalance(artwork.UserId),
                    TransactionDate = DateTime.Now,
                    BuyerId = userId
                };

                // Save transactions
                _transactionRepository.AddTransaction(userTransaction);
                _transactionRepository.AddTransaction(artistTransaction);

                // Update the session balance
                HttpContext.Session.SetString("Balance", _userRepository.GetCustomerBalance(userId).ToString());

                if (userBalanceUpdated && artistBalanceUpdated)
                {
                    // If purchase and balance update are successful, display a success message
                    TempData["SuccessMessage"] = "You have successfully purchased the artwork!";
                }
                else
                {
                    // If balance update fails, display an error message
                    TempData["ErrorMessage"] = "Purchase completed, but failed to update your balance. Please contact support.";
                }
            }
            else
            {
                // If purchase fails, display an error message
                TempData["ErrorMessage"] = "Failed to complete the purchase. Please try again.";
            }

            // Redirect to the details page of the artwork
            return RedirectToAction("Details", "Art", new { id = artId });
        }







    }
}
