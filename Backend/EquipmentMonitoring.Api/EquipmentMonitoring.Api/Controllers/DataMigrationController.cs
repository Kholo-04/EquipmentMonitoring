using EquipmentMonitoring.Api.Data;
using EquipmentMonitoring.Api.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace EquipmentMonitoring.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class DataMigrationController
        : ControllerBase
    {
        private readonly EquipmentMonitoringDbContext
            _dbContext;

        private readonly PasswordHasher<User>
            _passwordHasher =
                new PasswordHasher<User>();

        public DataMigrationController(
            EquipmentMonitoringDbContext dbContext
        )
        {
            _dbContext = dbContext;
        }

        [HttpPost("hash-users")]
        public async Task<IActionResult>
            HashExistingUsers()
        {
            var users =
                _dbContext.Users.ToList();

            foreach (var user in users)
            {
                // Skip already hashed passwords
                if (
                    user.PasswordHash.StartsWith(
                        "AQAAAA"
                    )
                )
                {
                    continue;
                }

                user.PasswordHash =
                    _passwordHasher.HashPassword(
                        user,
                        user.PasswordHash
                    );
            }

            await _dbContext.SaveChangesAsync();

            return Ok(
                "User passwords hashed successfully."
            );
        }
    }
}