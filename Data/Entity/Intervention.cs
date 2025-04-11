namespace ExamApi.Data.Entity
{
    public class Intervention
    {
        public int Id { get; set; }
        public DateTime ScheduledAt { get; set; }

        public int ClientId { get; set; }
        public Client Client { get; set; } = default!;

        public int ServiceTypeId { get; set; }
        public ServiceType ServiceType { get; set; } = default!;

        public ICollection<InterventionTechnician> TechnicianLinks { get; set; } = new List<InterventionTechnician>();
    }
}
