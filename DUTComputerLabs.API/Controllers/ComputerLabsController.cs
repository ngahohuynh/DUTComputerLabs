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
        [Authorize(Roles = "ADMIN, MANAGER")]
        public IEnumerable<ComputerLabForList> GetComputerLabs([FromQuery]LabParams labParams)
        {
            
            labParams.OwnerId = Convert.ToInt32(User.FindFirst(ClaimTypes.NameIdentifier).Value);

            var labs = _service.GetComputerLabs(labParams);

            Response.AddPagination(labs.CurrentPage, labs.PageSize, labs.TotalCount, labs.TotalPages);

            return _mapper.Map<IEnumerable<ComputerLabForList>>(labs);
        }

        [HttpGet("{id}")]
        // for LECTURER
        public ComputerLabForDetailed GetComputerLab(int id) => _service.GetComputerLab(id);

        [HttpGet("search")]
        // for LECTURER
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
            computerLab.OwnerId = Convert.ToInt32(User.FindFirst(ClaimTypes.NameIdentifier).Value);

            return _service.AddComputerLab(computerLab);
        }

        [HttpPut("{id}")]
        public ComputerLabForList UpdateComputerLab(int id, ComputerLabForInsert computerLab)
        {
            var ownerId = Convert.ToInt32(User.FindFirst(ClaimTypes.NameIdentifier).Value);
            var labOwner = _service.GetById(id).OwnerId;

            if(ownerId != 8 && labOwner != ownerId)
            {
                throw new ForbiddenException("Không có quyền chỉnh sửa phòng máy này");
            }
            
            computerLab.OwnerId = labOwner;

            var updatedLab = _service.UpdateComputerLab(id, computerLab);
            return updatedLab;
        }

        [HttpDelete("{id}")]
        public void DeleteComputerLab(int id)
        {
            var labToRemove = _service.GetById(id)
                ?? throw new BadRequestException("Phòng máy không tồn tại");

            var ownerId = Convert.ToInt32(User.FindFirst(ClaimTypes.NameIdentifier).Value);
            if(ownerId != 8 && labToRemove.OwnerId != ownerId)
            {
                throw new ForbiddenException("Không có quyền xóa phòng máy này");
            }

            _service.Delete(labToRemove);
        }
    }
}