using System;
using System.Collections.Generic;
using System.Security.Claims;
using AutoMapper;
using DUTComputerLabs.API.Dtos;
using DUTComputerLabs.API.Exceptions;
using DUTComputerLabs.API.Services;
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

        [HttpPost]
        public BookingForDetailed AddBooking(BookingForInsert booking)
        {
            booking.UserId = Convert.ToInt32(User.FindFirst(ClaimTypes.NameIdentifier).Value);

            return _service.AddBooking(booking);
        }

        [HttpPut("{id}")]
        public BookingForDetailed UpdateBooking(int id, BookingForInsert booking)
        {
            var userId = Convert.ToInt32(User.FindFirst(ClaimTypes.NameIdentifier).Value);

            if(_service.GetById(id).UserId != userId)
            {
                throw new ForbiddenException("Không có quyền chỉnh sửa lịch đặt phòng này");
            }

            booking.UserId = userId;

            return _service.UpdateBooking(id, booking);
        }



    }
}