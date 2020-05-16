using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using AutoMapper;
using DUTComputerLabs.API.Data;
using DUTComputerLabs.API.Dtos;
using DUTComputerLabs.API.Exceptions;
using DUTComputerLabs.API.Helpers;
using DUTComputerLabs.API.Models;
using DUTComputerLabs.API.Repositories;
using Microsoft.EntityFrameworkCore;

namespace DUTComputerLabs.API.Services
{
    public interface IUserService : IRepository<User>
    {
        PagedList<User> GetUsers(UserParams userParams);

        UserForDetailed GetUser(int id);

        UserForDetailed AddUser(UserForInsert user);

        UserForDetailed UpdateUser(int id, UserForInsert user);

        bool UsernameExists(string username);

        IEnumerable<Faculty> GetFaculties();
        
        UserForDetailed UpdateUserInfo(int id, UserForInsert user);

        void UpdatePassword(int id, PasswordToUpdate password);
    }

    public class UserService : Repository<User>, IUserService
    {
        private readonly DataContext _context;
        private readonly IMapper _mapper;

        public UserService(DataContext context, IMapper mapper) : base(context)
        {
            _context = context;
            _mapper = mapper;
        }

        public PagedList<User> GetUsers(UserParams userParams)
        {
            var users = _context.Users.Include(u => u.Role)
                                    .Where(u => string.Equals(u.Role.Name, userParams.RoleName))
                                    .AsQueryable();

            if(!string.IsNullOrEmpty(userParams.Name))
            {
                users = users.Where(u => u.Name.Contains(userParams.Name));
            }

            return PagedList<User>.Create(users, userParams.PageNumber, userParams.PageSize);
        }

        public UserForDetailed GetUser(int id)
        {
            var user = _context.Users.Include(u => u.Faculty).FirstOrDefault(u => u.Id == id);
            return _mapper.Map<UserForDetailed>(user);
        }

        public UserForDetailed AddUser(UserForInsert user)
        {
            var userToAdd = _mapper.Map<User>(user);
            userToAdd.Password = EncryptPassword(user.Password);
            userToAdd.Role = _context.Roles.FirstOrDefault(r => string.Equals(r.Name, user.Role));
            userToAdd.Faculty = _context.Faculties.Find(user.Faculty.Id);

            Add(userToAdd);

            return _mapper.Map<UserForDetailed>(userToAdd);
        }

        public UserForDetailed UpdateUser(int id, UserForInsert user)
        {
            var userToUpdate = GetById(id);

            _mapper.Map(user, userToUpdate);

            userToUpdate.Password = EncryptPassword(user.Password);
            userToUpdate.Role = _context.Roles.FirstOrDefault(r => string.Equals(r.Name, user.Role));
            userToUpdate.Faculty = _context.Faculties.Find(user.Faculty.Id);

            _context.SaveChanges();

            return _mapper.Map<UserForDetailed>(GetById(id));
        }

        public UserForDetailed UpdateUserInfo(int id, UserForInsert user)
        {
            var userToUpdate = GetById(id);
            _mapper.Map(user, userToUpdate);

            _context.SaveChanges();

            return _mapper.Map<UserForDetailed>(GetById(id));
        }

        public void UpdatePassword(int id, PasswordToUpdate password)
        {
            var userToUpdate = GetById(id);

            if(!string.Equals(userToUpdate.Password, EncryptPassword(password.OldPassword)))
                throw new BadRequestException($"Old password didn't match");

            userToUpdate.Password = EncryptPassword(password.NewPassword);
            
            _context.SaveChanges();
        }

        public bool UsernameExists(string username)
        {
            return _context.Users.Any(u => string.Equals(u.Username, username));
        }

        public IEnumerable<Faculty> GetFaculties()
        {
            return _context.Faculties;
        }

        private string EncryptPassword(string password)
        {
            var md5 = new MD5CryptoServiceProvider();

            md5.ComputeHash(ASCIIEncoding.ASCII.GetBytes(password));

            return Convert.ToBase64String(md5.Hash);
        }
    }
}