namespace EquipmentMonitoring.Api.Models
{
    public class PredictionResponse
    {
        public bool FailurePrediction { get; set; }

        public string Prediction { get; set; } = string.Empty;

        public double FailureProbability { get; set; }

        public double NoFailureProbability { get; set; }

        public string MaintenancePriority { get; set; } = string.Empty;

        public string Model { get; set; } = string.Empty;
    }
}