namespace EquipmentMonitoring.Api.Models
{
    public class DashboardResponse
    {
        public int TotalUsers { get; set; }

        public int TotalEquipment { get; set; }

        public int TotalPredictions { get; set; }

        public int HighPriorityPredictions { get; set; }

        public int ActiveEquipment { get; set; }

        public int InactiveEquipment { get; set; }

        public int LowPriorityPredictions { get; set; }
    }
}