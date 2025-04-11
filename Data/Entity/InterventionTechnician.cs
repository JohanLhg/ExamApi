namespace ExamApi.Data.Entity
{
    public class InterventionTechnician
    {
        public string TechnicianId { get; set; } = default!;
        public AppUser Technician { get; set; } = default!;

        public int InterventionId { get; set; }
        public Intervention Intervention { get; set; } = default!;
    }
}
