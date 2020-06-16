using System;
using System.Collections.Generic;
using System.Linq;
using AutoMapper;
using DUTComputerLabs.API.Data;
using DUTComputerLabs.API.Dtos;
using DUTComputerLabs.API.Exceptions;
using DUTComputerLabs.API.Models;
using DUTComputerLabs.API.Repositories;
using Microsoft.EntityFrameworkCore;

namespace DUTComputerLabs.API.Services
{
    public interface IFeedbackService : IRepository<Feedback>
    {
        IEnumerable<FeedbackForDetailed> GetFeedbacksForLab(int labId);
        
        FeedbackForDetailed AddFeedback(FeedbackForInsert feedback, int userId);
    }

    public class FeedbackService : Repository<Feedback>, IFeedbackService
    {
        private readonly DataContext _context;
        private readonly IMapper _mapper;

        public FeedbackService(DataContext context, IMapper mapper) : base(context)
        {
            _context = context;
            _mapper = mapper;
        }

        public IEnumerable<FeedbackForDetailed> GetFeedbacksForLab(int labId)
        {
            var feedbacks = _context.Feedbacks.Include(f => f.Booking).ThenInclude(b => b.User).Where(f => f.LabId == labId);
            return _mapper.Map<IEnumerable<FeedbackForDetailed>>(feedbacks);
        }
        
        public FeedbackForDetailed AddFeedback(FeedbackForInsert feedback, int userId)
        {
            var booking = _context.Bookings.Find(feedback.BookingId)
                ?? throw new BadRequestException("Lịch đặt phòng không tồn tại");

            if(booking.UserId != userId)
            {
                throw new ForbiddenException("Không có quyền phản hồi cho lịch đặt phòng này");
            }

            if(booking.Feedback != null)
            {
                throw new BadRequestException("Bạn đã phản hồi cho lịch đặt phòng này");
            }

            var feedbackToAdd = _mapper.Map<Feedback>(feedback);

            feedbackToAdd.Booking = _context.Bookings.Find(feedback.BookingId);
            feedbackToAdd.LabId = feedbackToAdd.Booking.LabId;
            feedbackToAdd.FeedbackDate = DateTime.Now;

            _context.Feedbacks.Add(feedbackToAdd);
            _context.SaveChanges();

            return _mapper.Map<FeedbackForDetailed>(feedbackToAdd);
        }
    }
}