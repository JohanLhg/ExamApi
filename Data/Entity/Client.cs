namespace ExamApi.Data.Entity
{
    public class Client
    {
        public int Id { get; set; }
        public string Name { get; set; } = default!;
        public string Address { get; set; } = default!;
        public string Email { get; set; } = default!;
        public ICollection<Intervention> Interventions { get; set; } = new List<Intervention>();
    }
}
