namespace ExamApi.Data.Entity
{
    public class ServiceType
    {
        public int Id { get; set; }
        public string Label { get; set; } = default!;
        public ICollection<Intervention> Interventions { get; set; } = new List<Intervention>();
    }

}
