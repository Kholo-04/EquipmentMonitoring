using EquipmentMonitoring.Api.Models;
using EquipmentMonitoring.Api.Repositories;
using Microsoft.AspNetCore.Mvc;

namespace EquipmentMonitoring.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class NotificationsController
        : ControllerBase
    {
        private readonly
            EmailNotificationRepository
            _repository;

        public NotificationsController(
            EmailNotificationRepository repository
        )
        {
            _repository = repository;
        }

        [HttpGet]
        public async Task<IActionResult>
            GetNotifications()
        {
            var notifications =
                await _repository
                    .GetNotificationsAsync();

            var response =
                notifications.Select(x =>
                    new NotificationResponse
                    {
                        NotificationId =
                            x.NotificationId,

                        Recipient =
                            x.Recipient,

                        Subject =
                            x.Subject,

                        Status =
                            x.Status,

                        DateSent =
                            x.DateSent
                    });

            return Ok(response);
        }
    }
}