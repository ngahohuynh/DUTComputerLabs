using System.Collections.Generic;
using System.Linq;
using AutoMapper;
using DUTComputerLabs.API.Data;
using DUTComputerLabs.API.Dtos;
using DUTComputerLabs.API.Helpers;
using DUTComputerLabs.API.Models;
using DUTComputerLabs.API.Repositories;
using Microsoft.EntityFrameworkCore;

namespace DUTComputerLabs.API.Services
{
    public interface INotificationService : IRepository<Notification>
    {
        IEnumerable<NotificationForDetailed> GetNotificationsForBooker(int bookerId);

        PagedList<Notification> GetNotificationsForManger(int managerId, PaginationParams paginationParams);

        void AddNotification(NotificationForInsert notification);        
    }

    public class NotificationService : Repository<Notification>, INotificationService
    {
        private readonly DataContext _context;
        private readonly IMapper _mapper;

        public NotificationService(DataContext context, IMapper mapper) : base(context)
        {
            _context = context;
            _mapper = mapper;
        }

        public IEnumerable<NotificationForDetailed> GetNotificationsForBooker(int bookerId)
        {
            var notifications = _context.Notifications.Include(n => n.Booking).ThenInclude(b => b.Lab)
                                        .Where(n => n.Booking.UserId == bookerId)
                                        .OrderByDescending(n => n.Id)
                                        .Take(10);
            return _mapper.Map<IEnumerable<NotificationForDetailed>>(notifications);
        }

        public PagedList<Notification> GetNotificationsForManger(int managerId, PaginationParams paginationParams)
        {
            var notifications = _context.Notifications.Include(n => n.Booking).ThenInclude(b => b.Lab)
                                        .Where(n => n.Booking.Lab.OwnerId == managerId)
                                        .OrderByDescending(n => n.Id);

            return PagedList<Notification>.Create(notifications, paginationParams.PageNumber, paginationParams.PageSize);
        }

        public void AddNotification(NotificationForInsert notification)
        {
            var notificationToAdd = _mapper.Map<Notification>(notification);
            notificationToAdd.Booking = _context.Bookings.Find(notification.BookingId);

            Add(notificationToAdd);
            _context.SaveChanges();
        }
        
    }
}