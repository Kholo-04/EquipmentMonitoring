namespace EquipmentMonitoring.Api.Models
{
    public class LoginResponse
    {
        public bool Success { get; set; }

        public string Username { get; set; }
            = string.Empty;

        public string Role { get; set; }
            = string.Empty;
    }
}