using System.ComponentModel.DataAnnotations;

namespace ArtPatio.Models
{
    public class UserProfile
    {
        public int Id { get; set; }

        [Required]
        public string Name { get; set; }

        [Required]
        [EmailAddress]
        public string Email { get; set; }

        [Required]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        [Required]
        [DataType(DataType.Password)]
        [Compare("Password", ErrorMessage = "Passwords do not match.")]
        public string ConfirmPassword { get; set; }

        public string Address { get; set; }

        public string Contact { get; set; }

        public string UserType { get; set; }

        public string Description { get; set; }
        public decimal Balance { get; set; }

        //public string ProfilePicture { get; set; }
    }
}
