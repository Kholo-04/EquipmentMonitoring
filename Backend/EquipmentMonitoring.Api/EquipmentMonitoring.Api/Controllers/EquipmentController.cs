using EquipmentMonitoring.Api.Entities;
using EquipmentMonitoring.Api.Models;
using EquipmentMonitoring.Api.Repositories;
using Microsoft.AspNetCore.Mvc;

namespace EquipmentMonitoring.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class EquipmentController : ControllerBase
    {
        private readonly EquipmentRepository _repository;

        private readonly AuditRepository _auditRepository;

        public EquipmentController(
            EquipmentRepository repository,
            AuditRepository auditRepository
        )
        {
            _repository = repository;

            _auditRepository = auditRepository;
        }

        [HttpGet]
        public async Task<IActionResult>
            GetEquipment()
        {
            var equipment =
                await _repository
                    .GetEquipmentAsync();

            var response =
                equipment.Select(x =>
                    new EquipmentResponse
                    {
                        EquipmentId =
                            x.EquipmentId,

                        EquipmentName =
                            x.EquipmentName,

                        EquipmentType =
                            x.EquipmentType,

                        Status =
                            x.Status
                    });

            return Ok(response);
        }

        [HttpPost]
        public async Task<IActionResult>
            CreateEquipment(
                CreateEquipmentRequest request
            )
        {
            var equipment = new Equipment
            {
                EquipmentName =
                    request.EquipmentName,

                EquipmentType =
                    request.EquipmentType,

                Status =
                    request.Status,

                CreatedDate =
                    DateTime.UtcNow
            };

            await _repository
                .CreateEquipmentAsync(
                    equipment
                );

            await _auditRepository
                .LogAsync(
                    "admin",
                    "Create Equipment",
                    "Equipment",
                    $"Created equipment: {equipment.EquipmentName}"
                );

            return Ok();
        }

        [HttpPut("{id}")]
        public async Task<IActionResult>
            UpdateEquipment(
                int id,
                UpdateEquipmentRequest request
            )
        {
            var updated =
                await _repository
                    .UpdateEquipmentAsync(
                        id,
                        request.EquipmentName,
                        request.EquipmentType,
                        request.Status
                    );

            if (!updated)
            {
                return NotFound();
            }

            await _auditRepository
                .LogAsync(
                    "admin",
                    "Update Equipment",
                    "Equipment",
                    $"Updated equipment: {request.EquipmentName}"
                );

            return Ok();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult>
            DeleteEquipment(
                int id
            )
        {
            var deleted =
                await _repository
                    .DeleteEquipmentAsync(id);

            if (!deleted)
            {
                return NotFound();
            }

            await _auditRepository
                .LogAsync(
                    "admin",
                    "Delete Equipment",
                    "Equipment",
                    $"Deleted Equipment Id: {id}"
                );

            return Ok();
        }
    }
}