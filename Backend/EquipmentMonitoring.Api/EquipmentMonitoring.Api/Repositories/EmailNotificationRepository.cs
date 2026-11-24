using EquipmentMonitoring.Api.Data;
using EquipmentMonitoring.Api.Entities;
using Microsoft.EntityFrameworkCore;

namespace EquipmentMonitoring.Api.Repositories
{
    public class EmailNotificationRepository
    {
        private readonly
            EquipmentMonitoringDbContext
            _dbContext;

        public EmailNotificationRepository(
            EquipmentMonitoringDbContext dbContext
        )
        {
            _dbContext = dbContext;
        }

        public async Task SaveNotificationAsync(
            EmailNotification notification
        )
        {
            _dbContext.EmailNotifications
                .Add(notification);

            await _dbContext
                .SaveChangesAsync();
        }

        public async Task<List<EmailNotification>>
            GetNotificationsAsync()
        {
            return await _dbContext
                .EmailNotifications
                .OrderByDescending(
                    x => x.DateSent
                )
                .ToListAsync();
        }

        public async Task SaveAsync(
    EmailNotification notification
)
        {
            _dbContext.EmailNotifications
                .Add(notification);

            await _dbContext
                .SaveChangesAsync();
        }
    }
}
