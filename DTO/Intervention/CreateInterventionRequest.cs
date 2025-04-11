namespace ExamApi.DTO.Intervention
{
    public class CreateInterventionRequest
    {
        public DateTime ScheduledAt { get; set; }

        public string ClientId { get; set; } = string.Empty;

        public int ServiceTypeId { get; set; }

        public List<string> TechnicianIds { get; set; } = new();
    }
}
