using NonProfit.DTOs;
using NonProfit.Models.Responses;

namespace NonProfit.Services.Interfaces
{
    public interface IAuthService
    {
        Task<AuthResponse> RegisterAsync(Register model);
        Task<AuthResponse> LoginAsync(LoginDTO model);
        Task<AuthResponse> RegisterAdminOrTherapistAsync(Register model, UserRole role);
       // Task EnsureUserExists(string userId);
        Task<AuthResponse> RefreshTokenAsync(string token, string refreshToken);
    }
}
