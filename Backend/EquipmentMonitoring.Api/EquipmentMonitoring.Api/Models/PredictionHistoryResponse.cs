namespace EquipmentMonitoring.Api.Models
{
    public class PredictionHistoryResponse
    {
        public int PredictionId { get; set; }

        public string Type { get; set; } = string.Empty;

        public bool FailurePrediction { get; set; }

        public double FailureProbability { get; set; }

        public string MaintenancePriority { get; set; }
            = string.Empty;

        public string ModelName { get; set; }
            = string.Empty;

        public DateTime CreatedDate { get; set; }
    }
}