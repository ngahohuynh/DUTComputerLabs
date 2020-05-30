using System;
using System.Collections.Generic;
using System.Security.Claims;
using AutoMapper;
using DUTComputerLabs.API.Dtos;
using DUTComputerLabs.API.Exceptions;
using DUTComputerLabs.API.Helpers;
using DUTComputerLabs.API.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace DUTComputerLabs.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class BookingsController : ControllerBase
    {
        private readonly IBookingService _service;
        private readonly IMapper _mapper;

        public BookingsController(IBookingService service, IMapper mapper)
        {
            _service = service;
            _mapper = mapper;
        }

        [HttpGet("manager")]
        [Authorize(Roles = "MANAGER")]
        [ServiceFilter(typeof(UpdateBookingStatus))]
        public IEnumerable<BookingForDetailed> GetBookingForManager([FromQuery]BookingParams bookingParams)
        {
            bookingParams.OwnerId = Convert.ToInt32(User.FindFirst(ClaimTypes.NameIdentifier).Value);

            var bookings = _service.GetBookingsForManager(bookingParams);

            Response.AddPagination(bookings.CurrentPage, bookings.PageSize, bookings.TotalCount, bookings.TotalPages);

            return _mapper.Map<IEnumerable<BookingForDetailed>>(bookings);
        }

        [HttpGet("booker")]
        [Authorize(Roles = "LECTURER")]
        [ServiceFilter(typeof(UpdateBookingStatus))]
        public IEnumerable<BookingForDetailed> GetBookingForBooker([FromQuery]BookingParams bookingParams)
        {
            bookingParams.BookerId = Convert.ToInt32(User.FindFirst(ClaimTypes.NameIdentifier).Value);

            var bookings = _service.GetBookingsForBooker(bookingParams);

            Response.AddPagination(bookings.CurrentPage, bookings.PageSize, bookings.TotalCount, bookings.TotalPages);

            return _mapper.Map<IEnumerable<BookingForDetailed>>(bookings);
        }


        [HttpPost]
        public void AddBooking(BookingForInsert booking)
        {
            booking.UserId = Convert.ToInt32(User.FindFirst(ClaimTypes.NameIdentifier).Value);
            _service.AddBooking(booking);
        }

        [HttpPut("{id}")]
        public void UpdateBooking(int id, BookingForInsert booking)
        {
            var userId = Convert.ToInt32(User.FindFirst(ClaimTypes.NameIdentifier).Value);

            if(_service.GetById(id).UserId != userId)
            {
                throw new ForbiddenException("Không có quyền chỉnh sửa lịch đặt phòng này");
            }

            booking.UserId = userId;

            _service.UpdateBooking(id, booking);
        }



    }
}