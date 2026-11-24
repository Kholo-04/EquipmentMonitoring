namespace EquipmentMonitoring.Api.Entities
{
    public class AuditLog
    {
        public int AuditLogId { get; set; }

        public string Username { get; set; }
            = string.Empty;

        public string Action { get; set; }
            = string.Empty;

        public string Module { get; set; }
            = string.Empty;

        public string Description { get; set; }
            = string.Empty;

        public DateTime DateCreated
        {
            get;
            set;
        }
    }
}