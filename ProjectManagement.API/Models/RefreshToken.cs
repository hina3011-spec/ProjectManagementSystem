namespace ProjectManagement.API.Models
{
    public class RefreshToken
    {
        public int RefreshTokenId { get; set; }

        public int EmployeeId { get; set; }

        public string Token { get; set; } = string.Empty;

        public DateTime ExpiryDate { get; set; }

        public bool IsRevoked { get; set; }

        public DateTime CreatedDate { get; set; }
    }
}