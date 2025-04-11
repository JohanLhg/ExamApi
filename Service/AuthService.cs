using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using ExamApi.Data.Entity;
using ExamApi.DTO.Auth;
using ExamApi.Exceptions;
using ExamApi.Resources;
using Microsoft.Extensions.Localization;
using Microsoft.IdentityModel.Tokens;
using ExamApi.Data.Repository;
using ExamApi.Data.Mappers;

namespace ExamApi.Service
{
    public class AuthService(
        IUserRepository authRepository,
        IConfiguration config,
        IStringLocalizer<SharedResources> localizer
    ) : IAuthService
    {
        private readonly IUserRepository _authRepository = authRepository;
        private readonly IConfiguration _config = config;
        private readonly IStringLocalizer<SharedResources> _localizer = localizer;

        public async Task<AuthResponse> RegisterAsync(RegisterRequest request)
        {
            var user = request.ToAppUser();
            var result = await _authRepository.CreateUser(user, request.Password);
            if (!result)
            {
                throw new AppException(_localizer["Registration_Failed"], 400);
            }

            return await GenerateTokensAsync(user);
        }

        public async Task<AuthResponse> RegisterTechnicianAsync(RegisterRequest request)
        {
            var user = request.ToAppUser();
            var result = await _authRepository.CreateUser(user, request.Password);
            if (!result)
            {
                throw new AppException(_localizer["Registration_Failed"], 400);
            }

            _authRepository.AddRoleToUser(user, "technician");

            return await GenerateTokensAsync(user);
        }

        public async Task<AuthResponse> RegisterClientAsync(RegisterRequest request)
        {
            var user = request.ToAppUser();
            var result = await _authRepository.CreateUser(user, request.Password);
            if (!result)
            {
                throw new AppException(_localizer["Registration_Failed"], 400);
            }

            _authRepository.AddRoleToUser(user, "client");

            return await GenerateTokensAsync(user);
        }

        public async Task<AuthResponse> LoginAsync(LoginRequest request)
        {
            var user = await _authRepository.FindByEmailAsync(request.Email);
            if (user == null || !await _authRepository.CheckPasswordAsync(user, request.Password))
            {
                throw new AppException(_localizer["Invalid_Credentials"], 401);
            }

            return await GenerateTokensAsync(user);
        }

        public async Task<AuthResponse> RefreshAsync(string refreshToken)
        {
            var user = await _authRepository.FindByRefreshTokenAsync(refreshToken);

            if (user == null || user.RefreshTokenExpiryTime < DateTime.UtcNow)
                throw new AppException(_localizer["Invalid_Token"], 401);

            return await GenerateTokensAsync(user);
        }

        private async Task<AuthResponse> GenerateTokensAsync(AppUser user)
        {
            var authClaims = new List<Claim>
        {
            new(ClaimTypes.NameIdentifier, user.Id),
            new(ClaimTypes.Name, user.UserName!),
            new(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
        };

            var roles = await _authRepository.GetRolesAsync(user);
            foreach (var role in roles)
            {
                authClaims.Add(new Claim(ClaimTypes.Role, role));
            }

            var jwtKey = _config["Jwt:Key"];
            var jwtIssuer = _config["Jwt:Issuer"];
            var jwtAudience = _config["Jwt:Audience"];

            var authSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtKey!));

            var token = new JwtSecurityToken(
                issuer: jwtIssuer,
                audience: jwtAudience,
                expires: DateTime.UtcNow.AddMinutes(30),
                claims: authClaims,
                signingCredentials: new SigningCredentials(authSigningKey, SecurityAlgorithms.HmacSha256)
            );

            var refreshToken = GenerateRefreshToken();
            user.RefreshToken = refreshToken;
            user.RefreshTokenExpiryTime = DateTime.UtcNow.AddDays(7);
            await _authRepository.UpdateUserAsync(user);

            return new AuthResponse
            {
                AccessToken = new JwtSecurityTokenHandler().WriteToken(token),
                RefreshToken = refreshToken
            };
        }

        private static string GenerateRefreshToken()
        {
            var randomBytes = new byte[64];
            using var rng = RandomNumberGenerator.Create();
            rng.GetBytes(randomBytes);
            return Convert.ToBase64String(randomBytes);
        }
    }
}