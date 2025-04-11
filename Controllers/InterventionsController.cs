using ExamApi.Data.Entity;
using ExamApi.DTO.Intervention;
using ExamApi.Service;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ExamApi.Controllers
{
    [ApiController]
    [Route("api/intervention")]
    [Authorize(Roles = "admin,technician")]
    public class InterventionsController(IInterventionService service) : ControllerBase
    {
        private readonly IInterventionService _service = service;

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CreateInterventionRequest request)
        {
            var intervention = await _service.CreateInterventionAsync(request);
            return Ok(intervention);
        }

        [HttpPost("type")]
        public async Task<IActionResult> Create([FromBody] CreateInterventionTypeRequest request)
        {
            var typeId = await _service.CreateInterventionTypeAsync(request);
            return Ok(new { typeId });
        }
    }
}