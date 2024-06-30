using System.ComponentModel.DataAnnotations;

namespace RandomApp1.Dtos
{
    public class RefreshTokenRequestDto
    {
        [Required]
        public string refreshTokenString { get; set; } = string.Empty;
    }
}