using EquipmentMonitoring.Api.Data;
using EquipmentMonitoring.Api.Entities;
using Microsoft.EntityFrameworkCore;

namespace EquipmentMonitoring.Api.Repositories
{
    public class AuditRepository
    {
        private readonly
            EquipmentMonitoringDbContext
            _dbContext;

        public AuditRepository(
            EquipmentMonitoringDbContext dbContext
        )
        {
            _dbContext = dbContext;
        }

        public async Task CreateAsync(
            AuditLog auditLog
        )
        {
            _dbContext.AuditLogs
                .Add(auditLog);

            await _dbContext
                .SaveChangesAsync();
        }

        public async Task<List<AuditLog>>
            GetAuditLogsAsync()
        {
            return await _dbContext.AuditLogs
                .OrderByDescending(
                    x => x.DateCreated
                )
                .ToListAsync();
        }

        public async Task LogAsync(
    string username,
    string action,
    string module,
    string description
)
        {
            var auditLog =
                new AuditLog
                {
                    Username =
                        username,

                    Action =
                        action,

                    Module =
                        module,

                    Description =
                        description,

                    DateCreated =
                        DateTime.UtcNow
                };

            _dbContext.AuditLogs
                .Add(auditLog);

            await _dbContext
                .SaveChangesAsync();
        }
    }
}