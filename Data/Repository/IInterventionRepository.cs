using ExamApi.Data.Entity;

namespace ExamApi.Data.Repository
{
    public interface IInterventionRepository
    {
        Task<Intervention> CreateIntervention(Intervention intervention);
        Task<InterventionType> CreateInterventionType(InterventionType type);
        Task<InterventionType> GetInterventionType(int typeId);
    }
}
