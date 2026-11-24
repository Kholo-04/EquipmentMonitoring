using EquipmentMonitoring.Api.Models;
using EquipmentMonitoring.Api.Repositories;
using Microsoft.AspNetCore.Mvc;

namespace EquipmentMonitoring.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuditController : ControllerBase
    {
        private readonly AuditRepository _repository;

        public AuditController(
            AuditRepository repository
        )
        {
            _repository = repository;
        }

        [HttpGet]
        public async Task<IActionResult>
            GetAuditLogs()
        {
            var logs =
                await _repository
                    .GetAuditLogsAsync();

            var response =
                logs.Select(x =>
                    new AuditResponse
                    {
                        AuditLogId =
                            x.AuditLogId,

                        Username =
                            x.Username,

                        Action =
                            x.Action,

                        Module =
                            x.Module,

                        Description =
                            x.Description,

                        DateCreated =
                            x.DateCreated
                    });

            return Ok(response);
        }
    }
}