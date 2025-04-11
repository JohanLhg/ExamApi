namespace ExamApi.Data.Entity
{
    public class Intervention
    {
        public int Id { get; set; }
        public DateTime ScheduledAt { get; set; }

        public string ClientId { get; set; } = string.Empty;
        public AppUser Client { get; set; } = default!;

        public int InterventionTypeId { get; set; }
        public InterventionType ServiceType { get; set; } = default!;

        public List<string> TechnicianIds { get; set; } = new List<string>();
        public List<AppUser> Technician { get; set; } = new List<AppUser>();
    }
}
