namespace EquipmentMonitoring.Api.Models
{
    public class PredictionRequest
    {
        public string Type { get; set; } = string.Empty;

        public double AirTemperature { get; set; }

        public double ProcessTemperature { get; set; }

        public double RotationalSpeed { get; set; }

        public double Torque { get; set; }

        public double ToolWear { get; set; }
    }
}