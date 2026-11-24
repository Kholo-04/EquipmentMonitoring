using EquipmentMonitoring.Api.Data;
using EquipmentMonitoring.Api.Entities;
using Microsoft.EntityFrameworkCore;

namespace EquipmentMonitoring.Api.Repositories
{
    public class EquipmentRepository
    {
        private readonly
            EquipmentMonitoringDbContext
            _dbContext;

        public EquipmentRepository(
            EquipmentMonitoringDbContext dbContext
        )
        {
            _dbContext = dbContext;
        }

        public async Task<List<Equipment>>
            GetEquipmentAsync()
        {
            return await _dbContext
                .Equipment
                .OrderBy(x =>
                    x.EquipmentName)
                .ToListAsync();
        }

        public async Task CreateEquipmentAsync(
    Equipment equipment
)
        {
            _dbContext.Equipment.Add(
                equipment
            );

            await _dbContext.SaveChangesAsync();
        }

        public async Task<bool>
    DeleteEquipmentAsync(
        int equipmentId
    )
        {
            var equipment =
                await _dbContext.Equipment
                    .FindAsync(equipmentId);

            if (equipment == null)
            {
                return false;
            }

            _dbContext.Equipment.Remove(
                equipment
            );

            await _dbContext.SaveChangesAsync();

            return true;
        }

        public async Task<bool>
    UpdateEquipmentAsync(
        int equipmentId,
        string equipmentName,
        string equipmentType,
        string status
    )
        {
            var equipment =
                await _dbContext.Equipment
                    .FindAsync(equipmentId);

            if (equipment == null)
            {
                return false;
            }

            equipment.EquipmentName =
                equipmentName;

            equipment.EquipmentType =
                equipmentType;

            equipment.Status =
                status;

            await _dbContext
                .SaveChangesAsync();

            return true;
        }
    }
}