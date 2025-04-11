using ExamApi.Data.Entity;

namespace ExamApi.DTO.Intervention
{
    public class InterventionDto
    {
        public int Id { get; set; }
        public DateTime ScheduledAt { get; set; }
        public string ClientName { get; set; } = default!;
        public string InterventionType { get; set; } = string.Empty;
        public List<string> TechnicianNames { get; set; } = new List<string>();
    }
}
