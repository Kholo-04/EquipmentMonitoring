using EquipmentMonitoring.Api.Data;
using EquipmentMonitoring.Api.Models;
using Microsoft.EntityFrameworkCore;

namespace EquipmentMonitoring.Api.Repositories
{
    public class DashboardRepository
    {
        private readonly
            EquipmentMonitoringDbContext
            _dbContext;

        public DashboardRepository(
            EquipmentMonitoringDbContext dbContext
        )
        {
            _dbContext = dbContext;
        }

        public async Task<DashboardResponse>
            GetDashboardAsync()
        {
            return new DashboardResponse
            {
                TotalUsers =
        await _dbContext.Users.CountAsync(),

                TotalEquipment =
        await _dbContext.Equipment.CountAsync(),

                TotalPredictions =
        await _dbContext.Predictions.CountAsync(),

                HighPriorityPredictions =
        await _dbContext.Predictions
            .CountAsync(
                x =>
                    x.MaintenancePriority
                        == "High"
            ),

                ActiveEquipment =
        await _dbContext.Equipment
            .CountAsync(
                x =>
                    x.Status == "Active"
            ),

                InactiveEquipment =
        await _dbContext.Equipment
            .CountAsync(
                x =>
                    x.Status == "Inactive"
            ),

                LowPriorityPredictions =
        await _dbContext.Predictions
            .CountAsync(
                x =>
                    x.MaintenancePriority
                        == "Low"
            )
            };

        }
    }
}