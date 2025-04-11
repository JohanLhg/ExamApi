using ExamApi.Data.Entity;
using ExamApi.Exceptions;
using ExamApi.DTO.Intervention;
using ExamApi.Data.Repository;

namespace ExamApi.Service
{
    public class InterventionService(
        IInterventionRepository interventionRepository,
        IUserRepository userRepository
    ) : IInterventionService
    {
        private readonly IInterventionRepository _interventionRepository = interventionRepository;
        private readonly IUserRepository _userRepository = userRepository;

        public async Task<InterventionDto> CreateInterventionAsync(CreateInterventionRequest request)
        {
            var intervention = new Intervention
            {
                ScheduledAt = request.ScheduledAt,
                ClientId = request.ClientId,
                InterventionTypeId = request.ServiceTypeId,
                TechnicianIds = request.TechnicianIds
            };

            
            var technicians = await _userRepository.GetUsers(request.TechnicianIds);

            if (technicians.Count != request.TechnicianIds.Count)
                throw new AppException("Un ou plusieurs techniciens sont introuvables", 400);

            intervention = await _interventionRepository.CreateIntervention(intervention);

            var client = await _userRepository.GetUser(intervention.ClientId);

            var type = await _interventionRepository.GetInterventionType(intervention.InterventionTypeId);

            return new InterventionDto
            {
                Id = intervention.Id,
                ScheduledAt = intervention.ScheduledAt,
                ClientName = client.FullName,
                InterventionType = type.Label,
                TechnicianNames = technicians.ConvertAll(t => t.FullName)
            };
        }

        public async Task<InterventionTypeDto> CreateInterventionTypeAsync(CreateInterventionTypeRequest request)
        {
            var entity = new InterventionType
            {
                Label = request.Label
            };

            entity = await _interventionRepository.CreateInterventionType(entity);

            return new InterventionTypeDto
            {
                Id = entity.Id,
                Label = entity.Label,
            };
        }
    }
}
