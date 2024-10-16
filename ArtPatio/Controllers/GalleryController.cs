using Microsoft.AspNetCore.Mvc;
using ArtPatio.Models;

namespace ArtPatio.Controllers
{
    public class GalleryController : BaseController
    {
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
            // Create a view model to pass the data to the view
            var userProfile = new UserInfoViewModel
            {
                Id = userId.Value,
                Name = userName,
                Email = userEmail,
                UserType = userType
            };
            return View(userProfile);
        }
    }
}
