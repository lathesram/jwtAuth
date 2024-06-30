namespace RandomApp1.Models
{
    public class IdentityUser
    {
        public int Id { get; set; }
        public string Email { get; set; } = string.Empty;
        public string Username { get; set; } = string.Empty;
        public byte[] PasswordHash { get; set; } = [];
        public byte[] PasswordSalt { get; set; } = [];
        public DateTime CreatedTime { get; set; }
    }
}