namespace RandomApp1.Models
{
    public class RefreshToken
    {
        public int Id { get; set; }
        public string Token { get; set; } = String.Empty;
        public int UserId { get; set; }
        public DateTime ExpiryDate { get; set; }
    }
}