using System.ComponentModel.DataAnnotations;

namespace RandomApp1.Dtos
{
    public class RegisterUserDto
    {
        [EmailAddress(ErrorMessage = "Enter valid email address")]
        [Required(ErrorMessage = "Email is required")]
        public string Email { get; set; } = string.Empty;

        [Required(ErrorMessage = "User name is required")]
        public string Username { get; set; } = string.Empty;

        [Required(ErrorMessage = "Password is required")]
        public string Password { get; set; } = string.Empty;
    }
}