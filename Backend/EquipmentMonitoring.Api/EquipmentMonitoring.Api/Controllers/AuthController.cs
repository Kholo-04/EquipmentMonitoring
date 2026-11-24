using EquipmentMonitoring.Api.Models;
using EquipmentMonitoring.Api.Repositories;
using Microsoft.AspNetCore.Mvc;

namespace EquipmentMonitoring.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly UserRepository _repository;

        public AuthController(
            UserRepository repository
        )
        {
            _repository = repository;
        }

        [HttpPost("login")]
        public async Task<IActionResult>
            Login(
                LoginRequest request
            )
        {
            var user =
                await _repository
                    .ValidateUserAsync(
                        request.Username,
                        request.Password
                    );

            if (user == null)
            {
                return Unauthorized(
                    new
                    {
                        message =
                            "Invalid username or password."
                    }
                );
            }

            return Ok(
                new LoginResponse
                {
                    Success = true,

                    Username =
                        user.Username,

                    Role =
                        user.Role
                }
            );
        }
    }
}