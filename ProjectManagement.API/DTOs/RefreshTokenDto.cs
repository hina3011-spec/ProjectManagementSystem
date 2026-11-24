using System.ComponentModel.DataAnnotations;

namespace ProjectManagement.API.DTOs
{
    public class RefreshTokenDto
    {
        [Required]
        public string RefreshToken { get; set; } = string.Empty;
    }
}