using ExamApi.Data.Entity;
using ExamApi.Exceptions;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace ExamApi.Data.Repository
{
    public class UserRepository(
        UserManager<AppUser> userManager, 
        RoleManager<IdentityRole> roleManager
    ) : IUserRepository
    {
        private readonly UserManager<AppUser> _userManager = userManager;
        private readonly RoleManager<IdentityRole> _roleManager = roleManager;

        public async Task<bool> CreateUser(AppUser user, string password)
        {
            var result = await _userManager.CreateAsync(user, password);
            return result.Succeeded;
        }

        public async void AddRoleToUser(AppUser user, string role)
        {
            if (!await _roleManager.RoleExistsAsync(role))
            {
                await _roleManager.CreateAsync(new IdentityRole(role));
            }

            await _userManager.AddToRoleAsync(user, role);
        }

        public Task<AppUser?> FindByEmailAsync(string email)
            => _userManager.FindByEmailAsync(email);

        public Task<bool> CheckPasswordAsync(AppUser user, string password)
            => _userManager.CheckPasswordAsync(user, password);

        public Task<IList<string>> GetRolesAsync(AppUser user)
            => _userManager.GetRolesAsync(user);

        public Task<AppUser?> FindByRefreshTokenAsync(string refreshToken)
            => _userManager.Users.FirstOrDefaultAsync(u => u.RefreshToken == refreshToken);

        public Task<IdentityResult> UpdateUserAsync(AppUser user)
            => _userManager.UpdateAsync(user);

        public async Task<List<AppUser>> GetUsers(List<string> userIds)
        {
            return await _userManager.Users
                .Where(u => userIds.Contains(u.Id))
                .ToListAsync();
        }

        public async Task<AppUser> GetUser(string userId)
        {
            return await _userManager.Users
                .FirstAsync(u => userId == u.Id);
        }
    }
}
