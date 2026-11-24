namespace ProjectManagement.API.DTOs
{
    public class LoginResponseDto
    {
        public string Token { get; set; } = string.Empty;

        public DateTime Expiry { get; set; }

        public string EmployeeName { get; set; } = string.Empty;

        public string Email { get; set; } = string.Empty;


        public string RefreshToken { get; set; } = string.Empty;

        public DateTime RefreshTokenExpiry { get; set; }

        //public string Role { get; set; } = string.Empty;
    }
}