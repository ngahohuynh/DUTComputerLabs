using System.Collections.Generic;
using AutoMapper;
using DUTComputerLabs.API.Dtos;
using DUTComputerLabs.API.Exceptions;
using DUTComputerLabs.API.Helpers;
using DUTComputerLabs.API.Models;
using DUTComputerLabs.API.Services;
using Microsoft.AspNetCore.Mvc;

namespace DUTComputerLabs.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UsersController : ControllerBase
    {
        private readonly IUserService _service;
        private readonly IMapper _mapper;

        public UsersController(IUserService service, IMapper mapper)
        {
            _service = service;
            _mapper = mapper;
        }

        [HttpGet]
        public IEnumerable<UserForList> GetUsers([FromQuery]UserParams userParams)
        {
            var users = _service.GetUsers(userParams);

            Response.AddPagination(users.CurrentPage, users.PageSize, users.TotalCount, users.TotalPages);

            return _mapper.Map<IEnumerable<UserForList>>(users);
        }

        [HttpGet("{id}")]
        public UserForDetailed GetUser(int id) => _service.GetUser(id);

        [HttpPost]
        public UserForDetailed AddUser(UserForInsert user)
        {
            if(_service.UsernameExists(user.Username))
                throw new BadRequestException("Username đã tồn tại. Vui lòng nhập Username khác");

            var createdUser = _service.AddUser(user);
            return createdUser;
        }

        [HttpPut("{id}")]
        public UserForDetailed UpdateUser(int id, UserForInsert user)
        {
            var existedUser = _service.GetById(id)
                ?? throw new BadRequestException("Người dùng không tồn tại");

            if(!string.Equals(user.Username, existedUser.Username))
            {
                throw new BadRequestException("Không thể thay đổi username");
            }

            var updatedUser = _service.UpdateUser(id, user);

            return updatedUser;
        }

        [HttpPut("{id}/info")]
        public UserForDetailed UpdateUserInfo(int id, UserForInsert user)
        {
            //check id of current principal

            if(!string.Equals(user.Username, _service.GetById(id).Username))
            {
                throw new BadRequestException("Không thể thay đổi username");
            }

            var updatedUser = _service.UpdateUser(id, user);

            return updatedUser;
        }

        [HttpPost("{id}/password")]
        public void UpdatePassword(int id, [FromBody]PasswordToUpdate password)
        {
            _service.UpdatePassword(id, password);
        }

        [HttpDelete("{id}")]
        public void DeleteUser(int id)
        {
            _service.Delete(_service.GetById(id));
        }

        [HttpGet("faculties")]
        public IEnumerable<Faculty> GetFaculties()
        {
            return _service.GetFaculties();
        }
    }
}