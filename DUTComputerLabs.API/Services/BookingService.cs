using System.Collections.Generic;
using System.Linq;
using AutoMapper;
using DUTComputerLabs.API.Data;
using DUTComputerLabs.API.Dtos;
using DUTComputerLabs.API.Exceptions;
using DUTComputerLabs.API.Helpers;
using DUTComputerLabs.API.Models;
using DUTComputerLabs.API.Repositories;
using Microsoft.EntityFrameworkCore;

namespace DUTComputerLabs.API.Services
{
    public interface IBookingService : IRepository<Booking>
    {
        PagedList<Booking> GetBookingsForManager(BookingParams bookingParams);

        PagedList<Booking> GetBookingsForBooker(BookingParams bookingParams);

        BookingForDetailed GetBooking(int id);

        void AddBooking(BookingForInsert booking);

        void UpdateBooking(int id, BookingForInsert booking);
    }

    public class BookingService : Repository<Booking>, IBookingService
    {
        private readonly DataContext _context;
        private readonly IMapper _mapper;

        public BookingService(DataContext context, IMapper mapper) : base(context)
        {
            _context = context;
            _mapper = mapper;
        }

        public PagedList<Booking> GetBookingsForManager(BookingParams bookingParams)
        {
            var bookings = _context.Bookings.Include(b => b.Lab)
                        .Where(b => b.Lab.OwnerId == bookingParams.OwnerId)
                                    // && b.BookingDate.Date == bookingParams.BookingDate.Date)
                        .AsQueryable();

            return PagedList<Booking>.Create(bookings, bookingParams.PageNumber, bookingParams.PageSize);
        }

        public PagedList<Booking> GetBookingsForBooker(BookingParams bookingParams)
        {
            var bookings = _context.Bookings.Include(b => b.Lab).ThenInclude(l => l.Owner)
                .Where(b => b.UserId == bookingParams.BookerId)
                .AsQueryable();
            return PagedList<Booking>.Create(bookings, bookingParams.PageNumber, bookingParams.PageSize);
        }

        public BookingForDetailed GetBooking(int id)
        {
            var booking = _context.Bookings.Where(b => b.Id == id);
            return _mapper.Map<BookingForDetailed>(booking);
        }
        
        public void AddBooking(BookingForInsert booking)
        {
            var bookingToAdd = _mapper.Map<Booking>(booking);
            bookingToAdd.User = _context.Users.Find(booking.UserId);
            bookingToAdd.Lab = _context.ComputerLabs.Find(booking.Lab.Id);
            bookingToAdd.Status = "Đã đặt";

            Add(bookingToAdd);
        }

        public void UpdateBooking(int id, BookingForInsert booking)
        {
            var bookingToUpdate = GetById(id)
                ?? throw new BadRequestException("Lịch đặt phòng này không tồn tại");
                
            _mapper.Map(booking, bookingToUpdate);

            bookingToUpdate.User = _context.Users.Find(booking.UserId);
            bookingToUpdate.Lab = _context.ComputerLabs.Find(booking.Lab.Id);
            bookingToUpdate.Status = "Đã cập nhật";

            _context.SaveChanges();
        }
    }
}