using EquipmentMonitoring.Api.Repositories;
using Microsoft.AspNetCore.Mvc;

namespace EquipmentMonitoring.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class DashboardController
        : ControllerBase
    {
        private readonly DashboardRepository
            _repository;

        public DashboardController(
            DashboardRepository repository
        )
        {
            _repository = repository;
        }

        [HttpGet]
        public async Task<IActionResult>
            GetDashboard()
        {
            var dashboard =
                await _repository
                    .GetDashboardAsync();

            return Ok(dashboard);
        }
    }
}