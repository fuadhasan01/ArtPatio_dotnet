namespace ArtPatio.Models
{
    public class Transaction
    {
        public int TransactionId { get; set; }
        public int UserId { get; set; }
        public int? ArtId { get; set; }
        public string TransactionType { get; set; }
        public decimal PreviousBalance { get; set; }
        public decimal UpdatedBalance { get; set; }
        public DateTime TransactionDate { get; set; }
        public int? BuyerId { get; set; } // Customer who bought the art
        public int? ArtistId { get; set; } 
    }
}
