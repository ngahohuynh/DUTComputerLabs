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
    public interface IBookingService : IRepository<Booking>
    {
        PagedList<Booking> GetBookings(BookingParams bookingParams);

        BookingForDetailed GetBooking(int id);

        BookingForDetailed AddBooking(BookingForInsert booking);
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

        public PagedList<Booking> GetBookings(BookingParams bookingParams)
        {
            var bookings = _context.Bookings.Include(b => b.Lab)
                        .Where(b => b.Lab.OwnerId == bookingParams.OwnerId
                                    && b.BookingDate.Date == bookingParams.BookingDate.Date)
                        .AsQueryable();

            return PagedList<Booking>.Create(bookings.AsQueryable(), bookingParams.PageNumber, bookingParams.PageSize);
        }

        public BookingForDetailed GetBooking(int id)
        {
            var booking = _context.Bookings.Where(b => b.Id == id);
            return _mapper.Map<BookingForDetailed>(booking);
        }
        
        public BookingForDetailed AddBooking(BookingForInsert booking)
        {
            var bookingToAdd = _mapper.Map<Booking>(booking);
            Add(bookingToAdd);

            return _mapper.Map<BookingForDetailed>(booking);
        }
    }
}