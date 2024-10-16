using ArtPatio.Models;

namespace ArtPatio.ViewModels
{
    public class UserProfileViewModel
    {
        public UserProfile UserProfile { get; set; }
        public List<Artwork> Artworks { get; set; }
        public List<Artwork> PurchasedArtworks { get; set; } // Added for customer's purchased artworks
    }
}
