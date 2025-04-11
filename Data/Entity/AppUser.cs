using Microsoft.AspNetCore.Identity;

namespace ExamApi.Data.Entity
{
    public class AppUser : IdentityUser
    {
        public string FullName { get; set; } = default!;
        public string? RefreshToken { get; set; }
        public DateTime RefreshTokenExpiryTime { get; set; }
        public ICollection<InterventionTechnician> TechnicianInterventions { get; set; } = new List<InterventionTechnician>();
    }
}
