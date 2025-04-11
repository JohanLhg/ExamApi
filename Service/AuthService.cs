using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using ExamApi.Data.Entity;
using ExamApi.DTO.Auth;
using ExamApi.Exceptions;
using ExamApi.Resource;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Localization;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;

namespace ExamApi.Service
{
    public class AuthService(UserManager<AppUser> userManager, IConfiguration config, IStringLocalizer<SharedResources> localizer) : IAuthService
    {
        private readonly UserManager<AppUser> _userManager = userManager;
        private readonly IConfiguration _config = config;
        private readonly IStringLocalizer<SharedResources> _localizer = localizer;

        public async Task<AuthResponse> RegisterAsync(RegisterRequest request)
        {
            var user = new AppUser
            {
                Email = request.Email,
                UserName = request.Email,
                FullName = request.FullName
            };

            var result = await _userManager.CreateAsync(user, request.Password);
            if (!result.Succeeded)
            {
                var errors = string.Join(", ", result.Errors.Select(e => e.Description));
                throw new AppException(_localizer["Registration_Failed"], 401);
            }

            var tokens = await GenerateTokensAsync(user);
            return tokens;
        }

        public async Task<AuthResponse> LoginAsync(LoginRequest request)
        {
            var user = await _userManager.FindByEmailAsync(request.Email);
            if (user == null || !await _userManager.CheckPasswordAsync(user, request.Password))
                throw new AppException(_localizer["Invalid_Credentials"], 401);

            return await GenerateTokensAsync(user);
        }

        public async Task<AuthResponse> RefreshAsync(string refreshToken)
        {
            var user = _userManager.Users.FirstOrDefault(u => u.RefreshToken == refreshToken);

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

            var roles = await _userManager.GetRolesAsync(user);
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
            await _userManager.UpdateAsync(user);

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
