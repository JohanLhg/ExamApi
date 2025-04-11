using ExamApi.Data.Entity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace ExamApi.Data
{
    public class AppDbContext : IdentityDbContext<AppUser>
    {
        public DbSet<Client> Clients => Set<Client>();
        public DbSet<ServiceType> ServiceTypes => Set<ServiceType>();
        public DbSet<Intervention> Interventions => Set<Intervention>();
        public DbSet<InterventionTechnician> InterventionTechnicians => Set<InterventionTechnician>();

        public AppDbContext(DbContextOptions<AppDbContext> options)
            : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            builder.Entity<InterventionTechnician>()
                .HasKey(it => new { it.InterventionId, it.TechnicianId });

            builder.Entity<InterventionTechnician>()
                .HasOne(it => it.Intervention)
                .WithMany(i => i.TechnicianLinks)
                .HasForeignKey(it => it.InterventionId);

            builder.Entity<InterventionTechnician>()
                .HasOne(it => it.Technician)
                .WithMany(u => u.TechnicianInterventions)
                .HasForeignKey(it => it.TechnicianId);
        }
    }
}
