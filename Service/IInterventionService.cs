using ExamApi.DTO.Intervention;

namespace ExamApi.Service
{
    public interface IInterventionService
    {
        Task<InterventionDto> CreateInterventionAsync(CreateInterventionRequest request);
        Task<InterventionTypeDto> CreateInterventionTypeAsync(CreateInterventionTypeRequest request);
    }
}
