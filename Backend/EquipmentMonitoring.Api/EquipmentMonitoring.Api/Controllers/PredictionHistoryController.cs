using EquipmentMonitoring.Api.Entities;
using EquipmentMonitoring.Api.Models;
using EquipmentMonitoring.Api.Repositories;
using Microsoft.AspNetCore.Mvc;

namespace EquipmentMonitoring.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class PredictionHistoryController
        : ControllerBase
    {
        private readonly PredictionRepository _repository;

        public PredictionHistoryController(
            PredictionRepository repository
        )
        {
            _repository = repository;
        }

        [HttpPost("save")]
        public async Task<IActionResult> SavePrediction(
            SavePredictionRequest request
        )
        {
            var prediction = new Prediction
            {
                Type = request.Type,
                FailurePrediction =
                    request.FailurePrediction,

                FailureProbability =
                    request.FailureProbability,

                MaintenancePriority =
                    request.MaintenancePriority,

                ModelName =
                    request.ModelName,

                CreatedDate =
                    DateTime.UtcNow
            };

            await _repository
                .SavePredictionAsync(
                    prediction
                );

            return Ok();
        }

        [HttpGet("history")]
        public async Task<IActionResult> GetHistory()
        {
            var predictions =
                await _repository
                    .GetPredictionHistoryAsync();

            var response =
                predictions.Select(x =>
                    new PredictionHistoryResponse
                    {
                        PredictionId =
                            x.PredictionId,

                        Type =
                            x.Type,

                        FailurePrediction =
                            x.FailurePrediction,

                        FailureProbability =
                            x.FailureProbability,

                        MaintenancePriority =
                            x.MaintenancePriority,

                        ModelName =
                            x.ModelName,

                        CreatedDate =
                            x.CreatedDate
                    });

            return Ok(response);
        }
    }
}