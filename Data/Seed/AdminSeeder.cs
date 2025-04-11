using ExamApi.Data.Entity;
using Microsoft.AspNetCore.Identity;

namespace ExamApi.Data.Seed
{
    public class AdminSeeder(IServiceProvider sp) : IDbSeeder
    {
        private readonly UserManager<AppUser> _userManager = sp.GetRequiredService<UserManager<AppUser>>();
        private readonly RoleManager<IdentityRole> _roleManager = sp.GetRequiredService<RoleManager<IdentityRole>>();

        public async Task SeedAsync()
        {
            var roleName = "admin";
            if (!await _roleManager.RoleExistsAsync(roleName))
                await _roleManager.CreateAsync(new IdentityRole(roleName));

            var admin = await _userManager.FindByEmailAsync("admin@example.com");
            if (admin == null)
            {
                admin = new AppUser
                {
                    UserName = "admin",
                    Email = "admin@example.com",
                    FullName = "Admin User",
                    EmailConfirmed = true
                };

                await _userManager.CreateAsync(admin, "Admin123!");
                await _userManager.AddToRoleAsync(admin, roleName);
            }
        }
    }
}
