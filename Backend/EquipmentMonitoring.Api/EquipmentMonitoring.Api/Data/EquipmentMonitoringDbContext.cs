using EquipmentMonitoring.Api.Entities;
using Microsoft.EntityFrameworkCore;

namespace EquipmentMonitoring.Api.Data
{
    public class EquipmentMonitoringDbContext
        : DbContext
    {
        public EquipmentMonitoringDbContext(
            DbContextOptions<EquipmentMonitoringDbContext> options
        )
            : base(options)
        {
        }

        public DbSet<User> Users =>
            Set<User>();

        public DbSet<Equipment> Equipment =>
            Set<Equipment>();

        public DbSet<Prediction> Predictions =>
            Set<Prediction>();

        public DbSet<EmailNotification>
            EmailNotifications =>
                Set<EmailNotification>();

        public DbSet<AuditLog>
    AuditLogs =>
        Set<AuditLog>();

        protected override void OnModelCreating(
            ModelBuilder modelBuilder
        )
        {
            modelBuilder.Entity<EmailNotification>()
                .HasKey(
                    x => x.NotificationId
                );

            base.OnModelCreating(
                modelBuilder
            );
        }
    }
}