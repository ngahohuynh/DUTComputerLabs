using System;
using System.Collections.Generic;
using System.Security.Claims;
using AutoMapper;
using DUTComputerLabs.API.Dtos;
using DUTComputerLabs.API.Exceptions;
using DUTComputerLabs.API.Helpers;
using DUTComputerLabs.API.Models;
using DUTComputerLabs.API.Services;
using Microsoft.AspNetCore.Authorization;
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
        [Authorize(Roles = "ADMIN")]
        public IEnumerable<UserForList> GetUsers([FromQuery]UserParams userParams)
        {
            var users = _service.GetUsers(userParams);

            Response.AddPagination(users.CurrentPage, users.PageSize, users.TotalCount, users.TotalPages);

            return _mapper.Map<IEnumerable<UserForList>>(users);
        }

        [HttpGet("{id}")]
        public UserForDetailed GetUser(int id) 
        {
            if(string.Equals(User.FindFirst(ClaimTypes.Role), "MANAGER") 
                && string.Equals(User.FindFirst(ClaimTypes.Role), "LECTURER") 
                && Convert.ToInt32(User.FindFirst(ClaimTypes.NameIdentifier).Value) != id)
            {
                throw new ForbiddenException("Không có quyền xem thông tin người dùng khác");
            }

            return _service.GetUser(id);
        }

        [HttpPost]
        [Authorize(Roles = "ADMIN")]
        public UserForDetailed AddUser(UserForInsert user)
        {
            if(_service.UsernameExists(user.Username))
                throw new BadRequestException("Username đã tồn tại. Vui lòng nhập Username khác");

            var createdUser = _service.AddUser(user);
            return createdUser;
        }

        [HttpPut("{id}")]
        [Authorize(Roles = "ADMIN")]
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
            if(Convert.ToInt32(User.FindFirst(ClaimTypes.NameIdentifier).Value) != id)
            {
                throw new ForbiddenException("Không có quyền chỉnh sửa thông tin người dùng khác");
            }

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
            if(Convert.ToInt32(User.FindFirst(ClaimTypes.NameIdentifier).Value) != id)
            {
                throw new ForbiddenException("Không có quyền chỉnh sửa thông tin người dùng khác");
            }

            _service.UpdatePassword(id, password);
        }

        [HttpDelete("{id}")]
        [Authorize(Roles = "ADMIN")]
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