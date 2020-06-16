using System;
using System.Collections.Generic;
using System.Security.Claims;
using AutoMapper;
using DUTComputerLabs.API.Dtos;
using DUTComputerLabs.API.Exceptions;
using DUTComputerLabs.API.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace DUTComputerLabs.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class FeedbacksController : ControllerBase
    {
        private readonly IFeedbackService _service;
        private readonly IMapper _mapper;

        public FeedbacksController(IFeedbackService service, IMapper mapper)
        {
            _service = service;
            _mapper = mapper;
        }

        [HttpGet("lab/{id}")]
        public IEnumerable<FeedbackForDetailed> GetFeedbacksForLab(int id)
        {
            return  _service.GetFeedbacksForLab(id);
        }

        [HttpPost]
        [Authorize(Roles = "LECTURER")]
        public FeedbackForDetailed AddFeedback(FeedbackForInsert feedback)
        {
            var userId = Convert.ToInt32(User.FindFirst(ClaimTypes.NameIdentifier).Value);

            return _service.AddFeedback(feedback, userId);
        }
    }
}