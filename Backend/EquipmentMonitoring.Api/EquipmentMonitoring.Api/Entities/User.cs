namespace EquipmentMonitoring.Api.Entities
{
    public class User
    {
        public int UserId { get; set; }

        public string Username { get; set; }
            = string.Empty;

        public string PasswordHash { get; set; }
            = string.Empty;

        public string Role { get; set; }
            = string.Empty;

        public DateTime CreatedDate { get; set;}

        public string EmailAddress { get; set; }
            = string.Empty;

    }
}