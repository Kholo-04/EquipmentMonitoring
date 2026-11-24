using EquipmentMonitoring.Api.Entities;
using EquipmentMonitoring.Api.Models;
using EquipmentMonitoring.Api.Repositories;
using Microsoft.AspNetCore.Mvc;

namespace EquipmentMonitoring.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UsersController : ControllerBase
    {
        private readonly UserRepository _repository;

        private readonly AuditRepository _auditRepository;

        public UsersController(
            UserRepository repository,
            AuditRepository auditRepository)
        {
            _repository = repository;

            _auditRepository = auditRepository;
        }

        [HttpGet]
        public async Task<IActionResult>
            GetUsers()
        {
            var users =
                await _repository.GetUsersAsync();

            var response =
                users.Select(x =>
                    new UserResponse
                    {
                        UserId = x.UserId,

                        Username = x.Username,

                        Role = x.Role,

                        EmailAddress = x.EmailAddress
                    });

            return Ok(response);
        }

        [HttpPost]
        public async Task<IActionResult>
            CreateUser(
                CreateUserRequest request
            )
        {
            var user = new User
            {
                Username =
                    request.Username,

                PasswordHash =
                    request.Password,

                Role =
                    request.Role,

                EmailAddress =
                    request.EmailAddress,

                CreatedDate =
                    DateTime.UtcNow
            };

            await _repository
                .CreateUserAsync(user);

            await _auditRepository
                .LogAsync(
                    "admin",
                    "Create User",
                    "User Management",
                    $"Created user: {user.Username}"
                );

            return Ok();
        }

        [HttpPut("{id}")]
        public async Task<IActionResult>
            UpdateUser(
                int id,
                UpdateUserRequest request
            )
        {
            var updated =
                await _repository
                    .UpdateUserAsync(
                        id,
                        request.Role,
                        request.EmailAddress
                    );

            if (!updated)
            {
                return NotFound();
            }

            await _auditRepository
                .LogAsync(
                    "admin",
                    "Update User",
                    "User Management",
                    $"Updated user: {id}"
                );

            return Ok();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult>
            DeleteUser(
                int id
            )
        {
            var deleted =
                await _repository
                    .DeleteUserAsync(id);

            if (!deleted)
            {
                return NotFound();
            }

            await _auditRepository
                .LogAsync(
                    "admin",
                    "Delete User",
                    "User Management",
                    $"Deleted user: {id}"
                );

            return Ok();
        }
    }
}