namespace EquipmentMonitoring.Api.Entities
{
    public class Equipment
    {
        public int EquipmentId { get; set; }

        public string EquipmentName { get; set; }
            = string.Empty;

        public string EquipmentType { get; set; }
            = string.Empty;

        public string Status { get; set; }
            = string.Empty;

        public DateTime CreatedDate
        {
            get;
            set;
        }
    }
}