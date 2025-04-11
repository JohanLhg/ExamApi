using ExamApi.Data.Entity;
using Microsoft.AspNetCore.Identity;

namespace ExamApi.Data.Repository
{
    public interface IUserRepository
    {
        Task<bool> CreateUser(AppUser user, string password);
        void AddRoleToUser(AppUser user, string role);
        Task<AppUser?> FindByEmailAsync(string email);
        Task<bool> CheckPasswordAsync(AppUser user, string password);
        Task<IList<string>> GetRolesAsync(AppUser user);
        Task<List<AppUser>> GetUsers(List<string> userIds);
        Task<AppUser> GetUser(string userId);
        Task<AppUser?> FindByRefreshTokenAsync(string refreshToken);
        Task<IdentityResult> UpdateUserAsync(AppUser user);
    }
}
