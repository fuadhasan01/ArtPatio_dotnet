using Microsoft.AspNetCore.Mvc;
using ArtPatio.Models;
using ArtPatio.Repositories;
using ArtPatio.ViewModels;


namespace ArtPatio.Controllers
{
    public class ArtistController : BaseController // Inherit from BaseController
    {
        private readonly ArtworkRepository _artworkRepository;
        private readonly UserRepository _userRepository;

        public ArtistController(ArtworkRepository artworkRepository, UserRepository userRepository)
        {
            _artworkRepository = artworkRepository;
            _userRepository = userRepository;
        }
        public IActionResult Index()
        {
            // Retrieve user data from session
            var userId = HttpContext.Session.GetInt32("Id");
            var userName = HttpContext.Session.GetString("Name");
            var userEmail = HttpContext.Session.GetString("UserEmail");
            var userType = HttpContext.Session.GetString("UserType");

            if (userId == null)
            {
                return RedirectToAction("Login", "Account");
            }

            // Store UserInfoViewModel in ViewData
            var userProfile = new UserInfoViewModel
            {
                Id = userId.Value,
                Name = userName,
                Email = userEmail,
                UserType = userType
            };

            // Store another model in ViewData (e.g., Artwork model)
            var artworks = _artworkRepository.GetAllArtworks().ToList();

            ViewBag.UserProfile = userProfile;
            ViewBag.ArtWorks = artworks;

            return View();
        }

        public IActionResult ViewArtist(int id)
        {
            var user = _userRepository.GetUserById(id);
            var artworks = _artworkRepository.GetAllArtworksByUserId(id);

            if (user == null) 
            {
                return NotFound();
            }

            var model = new UserProfileViewModel { };
            model.UserProfile = user;
            model.Artworks = artworks;


            return View(model);
        }

    }
}
