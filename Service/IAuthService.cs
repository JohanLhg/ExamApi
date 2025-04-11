using ExamApi.DTO.Auth;

namespace ExamApi.Service
{
    public interface IAuthService
    {
        Task<AuthResponse> RegisterAsync(RegisterRequest request);
        Task<AuthResponse> LoginAsync(LoginRequest request);
        Task<AuthResponse> RefreshAsync(string refreshToken);
    }
}
