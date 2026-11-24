namespace EquipmentMonitoring.Api.Entities
{
    public class EmailNotification
    {
        public int NotificationId { get; set; }

        public string Recipient { get; set; }
            = string.Empty;

        public string Subject { get; set; }
            = string.Empty;

        public string Status { get; set; }
            = string.Empty;

        public DateTime DateSent
        {
            get;
            set;
        }
    }
}