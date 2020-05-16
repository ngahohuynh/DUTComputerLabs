using System.Collections.Generic;
using AutoMapper;
using DUTComputerLabs.API.Dtos;
using DUTComputerLabs.API.Exceptions;
using DUTComputerLabs.API.Helpers;
using DUTComputerLabs.API.Services;
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
        public IEnumerable<ComputerLabForList> GetComputerLabs([FromQuery]LabParams labParams)
        {
            labParams.OwnerId = 1;

            var labs = _service.GetComputerLabs(labParams);

            Response.AddPagination(labs.CurrentPage, labs.PageSize, labs.TotalCount, labs.TotalPages);

            return _mapper.Map<IEnumerable<ComputerLabForList>>(labs);
        }

        [HttpPost]
        public ComputerLabForList AddComputerLab(ComputerLabForInsert computerLab)
        {
            var createdLab = _service.AddComputerLab(computerLab);
            return createdLab;
        }

        [HttpPut("{id}")]
        public ComputerLabForList UpdateComputerLab(int id, ComputerLabForInsert computerLab)
        {
            var updatedLab = _service.UpdateComputerLab(id, computerLab);
            return updatedLab;
        }

        [HttpDelete("{id}")]
        public void DeleteComputerLab(int id)
        {
            var labToRemove = _service.GetById(id)
                ?? throw new BadRequestException("Lab doesn't Exist");

            _service.Delete(labToRemove);
        }
    }
}