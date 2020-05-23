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
    public class ComputerLabsController : ControllerBase
    {
        private readonly IComputerLabService _service;
        private readonly IMapper _mapper;

        public ComputerLabsController(IComputerLabService service, IMapper mapper)
        {
            _service = service;
            _mapper = mapper;
        }

        [HttpGet]
        // [Authorize]
        public IEnumerable<ComputerLabForList> GetComputerLabs([FromQuery]LabParams labParams)
        {
            labParams.OwnerId = 2;
            // Get OwnerId from token
            // labParams.OwnerId = Convert.ToInt32(User.FindFirst(ClaimTypes.NameIdentifier).Value);

            var labs = _service.GetComputerLabs(labParams);

            Response.AddPagination(labs.CurrentPage, labs.PageSize, labs.TotalCount, labs.TotalPages);

            return _mapper.Map<IEnumerable<ComputerLabForList>>(labs);
        }

        [HttpGet("{id}")]
        public ComputerLabForDetailed GetComputerLab(int id) => _service.GetComputerLab(id);

        [HttpGet("search")]
        public IEnumerable<ComputerLabForList> SearchComputerLabsForBooking([FromQuery]LabParams labParams)
        {
            var labs = _service.SearchComputerLabsForBooking(labParams);

            // Response.AddPagination(labs.CurrentPage, labs.PageSize, labs.TotalCount, labs.TotalPages);
            foreach (var lab in labs)
            {
                lab.EditMode = labParams.EditMode;
            }

            return labs;
        }

        [HttpPost]
        public ComputerLabForList AddComputerLab(ComputerLabForInsert computerLab)
        {
            //get OwnerId from token

            var createdLab = _service.AddComputerLab(computerLab);
            return createdLab;
        }

        [HttpPut("{id}")]
        public ComputerLabForList UpdateComputerLab(int id, ComputerLabForInsert computerLab)
        {
            //Get OwnerId from token

            var updatedLab = _service.UpdateComputerLab(id, computerLab);
            return updatedLab;
        }

        [HttpDelete("{id}")]
        public void DeleteComputerLab(int id)
        {
            //Get OwnerId from token

            var labToRemove = _service.GetById(id)
                ?? throw new BadRequestException("Phòng máy không tồn tại");

            _service.Delete(labToRemove);
        }
    }
}