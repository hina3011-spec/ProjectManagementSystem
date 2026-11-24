using Microsoft.IdentityModel.Tokens;
using ProjectManagement.API.Models;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace ProjectManagement.API.Services
{
    public class TokenService
    {
        private readonly IConfiguration _configuration;

        public TokenService(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public (string Token, DateTime Expiry) GenerateToken(Employee employee)
        {
            var jwtSettings = _configuration.GetSection("Jwt");

            var secretKey = jwtSettings["Key"]!;

            var issuer = jwtSettings["Issuer"];
            var audience = jwtSettings["Audience"];

            var expiry = DateTime.UtcNow.AddMinutes(
                Convert.ToDouble(jwtSettings["ExpiryInMinutes"]));

            // Create user information (claims)
            var claims = new[]
            {
                new Claim(ClaimTypes.NameIdentifier,
                    employee.EmployeeId.ToString()),

                new Claim(ClaimTypes.Name,
                    employee.EmployeeName),

                new Claim(ClaimTypes.Email,
                    employee.Email),
                 new Claim(ClaimTypes.Role,
        employee.Role!.RoleName)
            };

            // Create secret key
            var key = new SymmetricSecurityKey(
                Encoding.UTF8.GetBytes(secretKey));

            // Create signing credentials
            var credentials = new SigningCredentials(
                key,
                SecurityAlgorithms.HmacSha256);

            // Create JWT
            var token = new JwtSecurityToken(
                issuer: issuer,
                audience: audience,
                claims: claims,
                expires: expiry,
                signingCredentials: credentials);

            // Convert JWT object into string
            var tokenString = new JwtSecurityTokenHandler()
                .WriteToken(token);

            return (tokenString, expiry);
        }
    }
}