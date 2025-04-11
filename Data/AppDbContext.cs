using ExamApi.Data.Entity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace ExamApi.Data
{
    public class AppDbContext : IdentityDbContext<AppUser>
    {
        public DbSet<InterventionType> InterventionTypes => Set<InterventionType>();
        public DbSet<Intervention> Interventions => Set<Intervention>();

        public AppDbContext(DbContextOptions<AppDbContext> options)
            : base(options)
        {
        }
    }
}
