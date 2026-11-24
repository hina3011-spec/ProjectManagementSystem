using ProjectManagement.API.DTOs;

namespace ProjectManagement.API.Interfaces
{
    public interface IAuthService
    {
        Task<LoginResponseDto?> LoginAsync(LoginDto dto);
        Task<LoginResponseDto?> RefreshTokenAsync(RefreshTokenDto dto);
        Task<bool> LogoutAsync(RefreshTokenDto dto);

    }
}