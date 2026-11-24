using System.Security.Cryptography;
using System.Text;

namespace ProjectManagement.API.Services
{
    public class RefreshTokenService
    {
        public string GenerateRefreshToken()
        {
            var randomBytes = new byte[64];

            using var randomNumberGenerator =
                RandomNumberGenerator.Create();

            randomNumberGenerator.GetBytes(randomBytes);

            return Convert.ToBase64String(randomBytes);
        }

        public string HashToken(string token)
        {
            var tokenBytes = Encoding.UTF8.GetBytes(token);

            var hashBytes = SHA256.HashData(tokenBytes);

            return Convert.ToBase64String(hashBytes);
        }
    }
}