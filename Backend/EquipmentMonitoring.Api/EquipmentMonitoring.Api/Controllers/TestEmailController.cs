using EquipmentMonitoring.Api.Services;
using Microsoft.AspNetCore.Mvc;

namespace EquipmentMonitoring.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class TestEmailController
        : ControllerBase
    {
        private readonly EmailService _emailService;

        public TestEmailController(
            EmailService emailService
        )
        {
            _emailService = emailService;
        }

        [HttpPost]
        public async Task<IActionResult>
            SendTestEmail()
        {
            await _emailService
                .SendEmailAsync(
                    "kholofelo.ramasodi@gmail.com",
                    "Equipment Monitoring Test Email",
                    "✅ Email service is working successfully."
                );

            return Ok(
                "Email sent successfully."
            );
        }
    }
}