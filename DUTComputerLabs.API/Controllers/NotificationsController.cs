using System;
using System.Collections.Generic;
using System.Security.Claims;
using AutoMapper;
using DUTComputerLabs.API.Dtos;
using DUTComputerLabs.API.Helpers;
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
        private readonly IMapper _mapper;

        public NotificationsController(INotificationService service, IMapper mapper)
        {
            _service = service;
            _mapper = mapper;
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
        public IEnumerable<NotificationForDetailed> GetNotificationsForManager([FromQuery]PaginationParams paginationParams)
        {
            var managerId = Convert.ToInt32(User.FindFirst(ClaimTypes.NameIdentifier).Value);

            var notifications = _service.GetNotificationsForManger(managerId, paginationParams);

            Response.AddPagination(notifications.CurrentPage, notifications.PageSize, notifications.TotalCount, notifications.TotalPages);

            return _mapper.Map<IEnumerable<NotificationForDetailed>>(notifications);
        }

        [HttpPost]
        [Authorize(Roles = "MANAGER")]
        public void AddNotification(NotificationForInsert notification) => _service.AddNotification(notification);
    }
}