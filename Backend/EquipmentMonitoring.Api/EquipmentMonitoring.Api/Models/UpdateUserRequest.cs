namespace EquipmentMonitoring.Api.Models
{
    public class UpdateUserRequest
    {
        public string Role { get; set; }
            = string.Empty;

        public string EmailAddress { get; set; }
            = string.Empty;
    }
}