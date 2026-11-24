using EquipmentMonitoring.Api.Data;
using EquipmentMonitoring.Api.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;

namespace EquipmentMonitoring.Api.Repositories
{
    public class UserRepository
    {
        private readonly EquipmentMonitoringDbContext _dbContext;
        private readonly PasswordHasher<User> _passwordHasher = new PasswordHasher<User>();

        public UserRepository(
            EquipmentMonitoringDbContext dbContext
        )
        {
            _dbContext = dbContext;
        }

        public async Task<User?>
    ValidateUserAsync(
        string username,
        string password
    )
        {
            var user =
                await _dbContext.Users
                    .FirstOrDefaultAsync(
                        x =>
                            x.Username == username
                    );

            if (user == null)
            {
                return null;
            }

            var result =
                _passwordHasher
                    .VerifyHashedPassword(
                        user,
                        user.PasswordHash,
                        password
                    );

            if (
                result ==
                PasswordVerificationResult.Success
            )
            {
                return user;
            }

            return null;
        }

        public async Task<List<User>>
   GetUsersAsync()
        {
            return await _dbContext.Users
                .OrderBy(x => x.Username)
                .ToListAsync();
        }

        public async Task CreateUserAsync(
    User user
)
        {
            user.PasswordHash =
                _passwordHasher.HashPassword(
                    user,
                    user.PasswordHash
                );

            _dbContext.Users.Add(user);

            await _dbContext.SaveChangesAsync();
        }

        public async Task<bool>
    UpdateUserAsync(
        int userId,
        string role,
        string emailAddress
    )
        {
            var user =
                await _dbContext.Users
                    .FindAsync(userId);

            if (user == null)
            {
                return false;
            }

            user.Role =
                role;

            user.EmailAddress =
                emailAddress;

            await _dbContext
                .SaveChangesAsync();

            return true;
        }


        public async Task<bool>
    DeleteUserAsync(
        int userId
    )
        {
            var user =
                await _dbContext.Users
                    .FindAsync(userId);

            if (user == null)
            {
                return false;
            }

            _dbContext.Users.Remove(user);

            await _dbContext.SaveChangesAsync();

            return true;
        }

        public async Task<List<User>>
    GetNotificationRecipientsAsync()
        {
            return await _dbContext.Users
                .Where(x =>
                    x.Role ==
                        "Maintenance Manager"
                    ||
                    x.Role ==
                        "Supervisor"
                )
                .ToListAsync();
        }

    }
}