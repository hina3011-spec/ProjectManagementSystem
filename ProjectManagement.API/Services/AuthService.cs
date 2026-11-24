using Microsoft.EntityFrameworkCore;
using ProjectManagement.API.Data;
using ProjectManagement.API.DTOs;
using ProjectManagement.API.Interfaces;
using ProjectManagement.API.Models;

namespace ProjectManagement.API.Services
{
    public class AuthService : IAuthService
    {
        private readonly ProjectManagementContext _context;
        private readonly PasswordService _passwordService;
        private readonly TokenService _tokenService;
        private readonly RefreshTokenService _refreshTokenService;
        public AuthService(
            ProjectManagementContext context,
            PasswordService passwordService,
            TokenService tokenService,
            RefreshTokenService refreshTokenService)
        {
            _context = context;
            _passwordService = passwordService;
            _tokenService = tokenService;
            _refreshTokenService = refreshTokenService;
        }

        public async Task<LoginResponseDto?> LoginAsync(LoginDto dto)
        {
            // Find employee by email
            var employee = await _context.Employees
                  .Include(e => e.Role)
                .FirstOrDefaultAsync(e => e.Email == dto.Email);

            // Check employee existence and account status
            if (employee == null || employee.IsActive != true)
            {
                return null;
            }

            // Verify password
            bool isPasswordValid = _passwordService.VerifyPassword(
                employee,
                dto.Password);

            if (!isPasswordValid)
            {
                return null;
            }

            // Generate JWT token
            var tokenResult = _tokenService.GenerateToken(employee);
            // Generate original refresh token
            var refreshToken = _refreshTokenService.GenerateRefreshToken();

            // Set refresh token expiry to 7 days
            var refreshTokenExpiry = DateTime.UtcNow.AddDays(7);

            // Create database record with hashed token
            var refreshTokenEntity = new RefreshToken
            {
                EmployeeId = employee.EmployeeId,

                Token = _refreshTokenService.HashToken(refreshToken),

                ExpiryDate = refreshTokenExpiry,

                IsRevoked = false,

                CreatedDate = DateTime.UtcNow
            };

            // Save refresh token in database
            _context.RefreshTokens.Add(refreshTokenEntity);

            await _context.SaveChangesAsync();



            // Return login response
            return new LoginResponseDto
            {
                Token = tokenResult.Token,
                Expiry = tokenResult.Expiry,
                EmployeeName = employee.EmployeeName,
                Email = employee.Email,
                RefreshToken = refreshToken,
                RefreshTokenExpiry = refreshTokenExpiry
            };

        }

        public async Task<LoginResponseDto?> RefreshTokenAsync(
    RefreshTokenDto dto)
        {
            // 1. Hash the token received from client
            var hashedToken = _refreshTokenService.HashToken(
                dto.RefreshToken);

            // 2. Find a valid refresh token
            var storedToken = await _context.RefreshTokens
                .FirstOrDefaultAsync(t =>
                    t.Token == hashedToken &&
                    !t.IsRevoked &&
                    t.ExpiryDate > DateTime.UtcNow);

            if (storedToken == null)
                return null;

            // 3. Get employee with role
            var employee = await _context.Employees
                .Include(e => e.Role)
                .FirstOrDefaultAsync(e =>
                    e.EmployeeId == storedToken.EmployeeId);

            if (employee == null ||
                employee.IsActive != true ||
                employee.Role == null)
            {
                return null;
            }

            // 4. Generate new access token
            var accessToken = _tokenService.GenerateToken(employee);

            // 5. Generate new refresh token
            var newRefreshToken =
                _refreshTokenService.GenerateRefreshToken();

            var refreshTokenExpiry = DateTime.UtcNow.AddDays(7);

            // 6. Revoke old refresh token
            storedToken.IsRevoked = true;

            // 7. Store new refresh token hash
            var newTokenEntity = new RefreshToken
            {
                EmployeeId = employee.EmployeeId,
                Token = _refreshTokenService.HashToken(newRefreshToken),
                ExpiryDate = refreshTokenExpiry,
                IsRevoked = false,
                CreatedDate = DateTime.UtcNow
            };

            _context.RefreshTokens.Add(newTokenEntity);

            // 8. Save changes
            await _context.SaveChangesAsync();

            // 9. Return new tokens
            return new LoginResponseDto
            {
                Token = accessToken.Token,
                Expiry = accessToken.Expiry,
                RefreshToken = newRefreshToken,
                RefreshTokenExpiry = refreshTokenExpiry,
                EmployeeName = employee.EmployeeName,
                Email = employee.Email
            };
        }
        public async Task<bool> LogoutAsync(RefreshTokenDto dto)
        {
            // 1. Hash the refresh token received from client
            var hashedToken = _refreshTokenService.HashToken(
                dto.RefreshToken);

            // 2. Find the token in database
            var storedToken = await _context.RefreshTokens
                .FirstOrDefaultAsync(t =>
                    t.Token == hashedToken &&
                    !t.IsRevoked);

            // 3. Check whether token exists
            if (storedToken == null)
                return false;

            // 4. Revoke the refresh token
            storedToken.IsRevoked = true;

            // 5. Save changes
            await _context.SaveChangesAsync();

            return true;
        }
    }
}