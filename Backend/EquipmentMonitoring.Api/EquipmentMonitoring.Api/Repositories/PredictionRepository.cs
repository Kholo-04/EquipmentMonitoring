using EquipmentMonitoring.Api.Data;
using EquipmentMonitoring.Api.Entities;
using Microsoft.EntityFrameworkCore;

namespace EquipmentMonitoring.Api.Repositories
{
    public class PredictionRepository
    {
        private readonly EquipmentMonitoringDbContext _dbContext;

        public PredictionRepository(
            EquipmentMonitoringDbContext dbContext
        )
        {
            _dbContext = dbContext;
        }

        public async Task SavePredictionAsync(
            Prediction prediction
        )
        {
            _dbContext.Predictions.Add(
                prediction
            );

            await _dbContext.SaveChangesAsync();
        }

        public async Task<List<Prediction>>
            GetPredictionHistoryAsync()
        {
            return await _dbContext
                .Predictions
                .OrderByDescending(
                    x => x.CreatedDate
                )
                .ToListAsync();
        }
    }
}