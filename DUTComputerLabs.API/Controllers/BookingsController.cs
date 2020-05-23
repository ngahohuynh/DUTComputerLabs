using System.Collections.Generic;
using AutoMapper;
using DUTComputerLabs.API.Dtos;
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
            // get userId from token

            // booking.UserId = 5;

            return _service.AddBooking(booking);
        }

        [HttpPut("{id}")]
        public BookingForDetailed UpdateBooking(int id, BookingForInsert booking)
        {
            return _service.UpdateBooking(id, booking);
        }



    }
}