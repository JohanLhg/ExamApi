using System.Threading.Tasks;
using ExamApi.Data.Entity;
using Microsoft.EntityFrameworkCore;

namespace ExamApi.Data.Repository
{
    public class InterventionRepository(AppDbContext context) : IInterventionRepository
    {
        private readonly AppDbContext _context = context;

        public async Task<Intervention> CreateIntervention(Intervention intervention)
        {
            _context.Interventions.Add(intervention);
            await _context.SaveChangesAsync();
            return intervention;
        }

        public async Task<InterventionType> CreateInterventionType(InterventionType type)
        {
            _context.InterventionTypes.Add(type);
            await _context.SaveChangesAsync();
            return type;
        }

        public async Task<InterventionType> GetInterventionType(int typeId)
        {
            return await _context.InterventionTypes.FirstAsync(s => s.Id == typeId);
        }
    }
}
