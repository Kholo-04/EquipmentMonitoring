namespace EquipmentMonitoring.Api.Models
{
    public class EquipmentResponse
    {
        public int EquipmentId { get; set; }

        public string EquipmentName { get; set; }
            = string.Empty;

        public string EquipmentType { get; set; }
            = string.Empty;

        public string Status { get; set; }
            = string.Empty;
    }
}