using ArtPatio.Models;
using ArtPatio.Repositories;
using ArtPatio.ViewModels;
using Microsoft.AspNetCore.Mvc;

namespace ArtPatio.Controllers
{
    public class ProfileController : BaseController
    {
        private readonly UserRepository _userRepository;
        private readonly ArtworkRepository _artworkRepository;

        public ProfileController(UserRepository userRepository, ArtworkRepository artworkRepository)
        {
            _userRepository = userRepository;
            _artworkRepository = artworkRepository;
        }

        // GET: /Profile/View
        public IActionResult ViewProfile()
        {
            if (HttpContext.Session.GetInt32("Id") != null)
            {
                // Get the logged-in user's ID from session
                var userId = HttpContext.Session.GetInt32("Id").GetValueOrDefault();

                // Retrieve user profile data
                var userProfile = _userRepository.GetUserById(userId);

                // Prepare model based on user type
                var model = new UserProfileViewModel
                {
                    UserProfile = userProfile
                };

                // If the user is an artist, retrieve their artworks
                if (userProfile.UserType == "Artist")
                {
                    var artistArtworks = _artworkRepository.GetAllArtworksByUserId(userId);
                    model.Artworks = artistArtworks;
                }
                // If the user is a customer, retrieve their purchased artworks
                else if (userProfile.UserType == "Customer")
                {
                    var purchasedArtworks = _artworkRepository.GetPurchasedArtworksByUserId(userId);
                    model.PurchasedArtworks = purchasedArtworks;
                }

                return View(model);
            }
            return RedirectToAction("Login", "Account");
        }
    }
}
