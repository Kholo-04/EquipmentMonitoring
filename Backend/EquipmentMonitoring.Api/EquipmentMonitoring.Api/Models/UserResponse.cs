namespace EquipmentMonitoring.Api.Models
{
    public class UserResponse
    {
        public int UserId { get; set; }

        public string Username { get; set; }
            = string.Empty;

        public string Role { get; set; }
            = string.Empty;

        public string EmailAddress { get; set; }
            = string.Empty;
    }
}