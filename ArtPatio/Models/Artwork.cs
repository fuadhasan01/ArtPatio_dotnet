namespace ArtPatio.Models
{
    public class Artwork
    {
        public int ArtId { get; set; } // Unique Art ID
        public string ArtName { get; set; }
        public string ArtMaterial { get; set; }
        public string ArtDetails { get; set; }
        public string ArtImage { get; set; } // This should ideally store the file path or URL
        public int UserId { get; set; } 
        public string UserName { get; set; }
        public decimal Price { get; set; }
        public string Status { get; set; }
    }
}
