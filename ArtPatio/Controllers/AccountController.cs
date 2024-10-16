using ArtPatio.Models;
using ArtPatio.Repositories;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;


namespace ArtPatio.Controllers
{
    public class AccountController : Controller
    {
        private readonly UserProfileRepository _userProfileRepository;

        public AccountController(UserProfileRepository userProfileRepository)
        {
            _userProfileRepository = userProfileRepository;
        }

        

        // GET: /Account/Signup
        public IActionResult Signup()
        {
            
            // Check if the user is already logged in
            if (HttpContext.Session.GetInt32("Id") != null)
            {
                return RedirectToAction("Index", HttpContext.Session.GetString("UserType")); // Redirect to home or any other page
            }
            return View();
        }

        [HttpPost]
        public IActionResult Signup(UserProfile model)
        {
            if (ModelState.IsValid)
            {
                // Validate the email format
                var emailErrors = ValidateEmail(model.Email);
                if (emailErrors.Any())
                {
                    ViewData["EmailError"] = emailErrors.First();
                    ModelState.AddModelError("Email", "Invalid email format.");
                    return View(model);
                }

                // Check if the email already exists
                if (_userProfileRepository.IsEmailExists(model.Email))
                {
                    ModelState.AddModelError("Email", "Email is already registered.");
                    return View(model);
                }

                // Validate password strength
                var passwordErrors = ValidatePasswordStrength(model.Password);
                if (passwordErrors.Any())
                {
                    ViewData["PasswordError"] = passwordErrors.First();
                    ModelState.AddModelError("Password", "Invalid password.");
                    return View(model);
                }
                model.Balance = 0;
                // Create the user profile
                _userProfileRepository.CreateUserProfile(model);
                TempData["SuccessMessage"] = "Account created successfully!";
                return RedirectToAction("Login", "Account"); // Redirect to the login page
            }

            return View(model);
        }

        // GET: /Account/Login
        public IActionResult Login()
        {
            if (HttpContext.Session.GetInt32("Id") != null)
            {
                return RedirectToAction("Index", HttpContext.Session.GetString("UserType")); // Redirect to home or any other page
            }
            return View();
        }

        [HttpPost]
        public IActionResult Login(LoginViewModel model)
        {
            if (ModelState.IsValid)
            {
                // Check if user exists in the database
                var user = _userProfileRepository.GetUserByEmailAndPassword(model.Email, model.Password);

                if (user != null)
                {
                    // Set session values for user info
                    HttpContext.Session.SetInt32("Id", user.Id);
                    HttpContext.Session.SetString("Name", user.Name);
                    HttpContext.Session.SetString("UserEmail", user.Email);
                    HttpContext.Session.SetString("UserType", user.UserType);
                    HttpContext.Session.SetString("Balance", user.Balance.ToString());

                    // Redirect based on user type
                    switch (user.UserType)
                    {
                        case "Customer":
                            return RedirectToAction("Index", "Customer");
                        case "Artist":
                            return RedirectToAction("Index", "Artist");
                        case "Gallery":
                            return RedirectToAction("Index", "Gallery");
                        default:
                            return RedirectToAction("Index", "Home"); // Fallback redirection
                    }
                }
                else
                {
                    TempData["ErrorMessage"] = "Invalid email or password.";
                }
            }

            return View(model);
        }

        // GET: /Account/Logout
        public IActionResult Logout()
        {
            // Clear session
            HttpContext.Session.Clear();
            return RedirectToAction("Login", "Account");
        }

        // Method to validate password strength
        private List<string> ValidatePasswordStrength(string password)
        {
            var errors = new List<string>();

            if (string.IsNullOrWhiteSpace(password))
            {
                errors.Add("Password cannot be empty.");
                return errors;
            }

            if (password.Length < 8)
                errors.Add("Password must be at least 8 characters long.");
            if (!password.Any(char.IsUpper))
                errors.Add("Password must contain at least one uppercase letter.");
            if (!password.Any(char.IsLower))
                errors.Add("Password must contain at least one lowercase letter.");
            if (!password.Any(char.IsDigit))
                errors.Add("Password must contain at least one number.");
            if (!password.Any(c => !char.IsLetterOrDigit(c)))
                errors.Add("Password must contain at least one special character (e.g., @, #, $, etc.).");

            return errors;
        }

        // Method to validate email format
        private List<string> ValidateEmail(string email)
        {
            var errors = new List<string>();
            var emailAddress = new EmailAddressAttribute();

            if (string.IsNullOrWhiteSpace(email))
            {
                errors.Add("Email cannot be empty.");
            }
            else if (!emailAddress.IsValid(email))
            {
                errors.Add("Invalid email format.");
            }

            return errors;
        }
    }
}
