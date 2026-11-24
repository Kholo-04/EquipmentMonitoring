namespace EquipmentMonitoring.Api.Models
{
    public class NotificationResponse
    {
        public int NotificationId { get; set; }

        public string Recipient { get; set; }
            = string.Empty;

        public string Subject { get; set; }
            = string.Empty;

        public string Status { get; set; }
            = string.Empty;

        public DateTime DateSent { get; set; }
    }
}