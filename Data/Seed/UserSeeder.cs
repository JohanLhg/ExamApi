using ExamApi.Data.Entity;
using Microsoft.AspNetCore.Identity;

namespace ExamApi.Data.Seed
{
    public class UserSeeder(IServiceProvider sp) : IDbSeeder
    {
        private readonly UserManager<AppUser> _userManager = sp.GetRequiredService<UserManager<AppUser>>();
        private readonly RoleManager<IdentityRole> _roleManager = sp.GetRequiredService<RoleManager<IdentityRole>>();

        public async Task SeedAsync()
        {
            CreateRole("admin");
            CreateRole("client");
            CreateRole("technician");

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
                await _userManager.AddToRoleAsync(admin, "admin");
            }

            var client = await _userManager.FindByEmailAsync("client@example.com");
            if (client == null)
            {
                client = new AppUser
                {
                    UserName = "client",
                    Email = "client@example.com",
                    FullName = "Client User",
                    EmailConfirmed = true
                };

                await _userManager.CreateAsync(admin, "Client123!");
                await _userManager.AddToRoleAsync(admin, "client");
            }

            var tech = await _userManager.FindByEmailAsync("tech@example.com");
            if (tech == null)
            {
                tech = new AppUser
                {
                    UserName = "tech",
                    Email = "tech@example.com",
                    FullName = "Tech User",
                    EmailConfirmed = true
                };

                await _userManager.CreateAsync(admin, "Tech123!");
                await _userManager.AddToRoleAsync(admin, "tech");
            }
        }

        public async void CreateRole(string roleName)
        {
            if (!await _roleManager.RoleExistsAsync(roleName))
                await _roleManager.CreateAsync(new IdentityRole(roleName));
        }
    }
}
