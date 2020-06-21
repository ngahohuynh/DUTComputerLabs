using System;
using System.Collections.Generic;
using System.Security.Claims;
using DUTComputerLabs.API.Dtos;
using DUTComputerLabs.API.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace DUTComputerLabs.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class NotificationsController : ControllerBase
    {
        private readonly INotificationService _service;

        public NotificationsController(INotificationService service)
        {
            _service = service;
        }

        [HttpGet("booker")]
        [Authorize(Roles = "LECTURER")]
        public IEnumerable<NotificationForDetailed> GetNotificationsForBooker()
        {
            var bookerId = Convert.ToInt32(User.FindFirst(ClaimTypes.NameIdentifier).Value);

            return _service.GetNotificationsForBooker(bookerId);
        }

        [HttpGet("manager")]
        [Authorize(Roles = "MANAGER")]
        public IEnumerable<NotificationForDetailed> GetNotificationsForManager()
        {
            var managerId = Convert.ToInt32(User.FindFirst(ClaimTypes.NameIdentifier).Value);

            return _service.GetNotificationsForManger(managerId);
        }

        [HttpPost]
        [Authorize(Roles = "MANAGER")]
        public void AddNotification(NotificationForInsert notification) => _service.AddNotification(notification);
    }
}