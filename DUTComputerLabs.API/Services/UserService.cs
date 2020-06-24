using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using AutoMapper;
using CloudinaryDotNet;
using CloudinaryDotNet.Actions;
using DUTComputerLabs.API.Data;
using DUTComputerLabs.API.Dtos;
using DUTComputerLabs.API.Exceptions;
using DUTComputerLabs.API.Helpers;
using DUTComputerLabs.API.Models;
using DUTComputerLabs.API.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;

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
        
        // UserForDetailed UpdateUserInfo(int id, UserForInsert user);

        void UpdatePassword(int id, PasswordToUpdate password);
    }

    public class UserService : Repository<User>, IUserService
    {
        private readonly DataContext _context;
        private readonly IMapper _mapper;
        private readonly IOptions<CloudinarySettings> _cloudinaryConfig;
        private Cloudinary _cloudinary;

        public UserService(DataContext context, IMapper mapper, IOptions<CloudinarySettings> cloudinaryConfig) : base(context)
        {
            _context = context;
            _mapper = mapper;
            _cloudinaryConfig = cloudinaryConfig;

            Account acc = new Account(
                _cloudinaryConfig.Value.CloudName,
                _cloudinaryConfig.Value.ApiKey,
                _cloudinaryConfig.Value.ApiSecret
            );

            _cloudinary = new Cloudinary(acc);
        }

        public PagedList<User> GetUsers(UserParams userParams)
        {
            var users = _context.Users.Include(u => u.Role)
                                    .Where(u => string.Equals(u.Role.Name, userParams.RoleName))
                                    .AsQueryable();

            if(!string.IsNullOrEmpty(userParams.Name))
            {
                users = users.Where(u => u.Name.StartsWith(userParams.Name));
            }

            return PagedList<User>.Create(users, userParams.PageNumber, userParams.PageSize);
        }

        public UserForDetailed GetUser(int id)
        {
            var user = _context.Users.Include(u => u.Faculty).Include(u => u.Role).FirstOrDefault(u => u.Id == id)
                ?? throw new BadRequestException("Người dùng không tồn tại");
            return _mapper.Map<UserForDetailed>(user);
        }

        public UserForDetailed AddUser(UserForInsert user)
        {
            var userToAdd = _mapper.Map<User>(user);
            userToAdd.Password = EncryptPassword(user.Password);
            userToAdd.Role = _context.Roles.FirstOrDefault(r => string.Equals(r.Name, user.Role));
            userToAdd.Faculty = _context.Faculties.Find(user.Faculty.Id);
            userToAdd.PhotoUrl = null;
            userToAdd.PhotoPublicId = null;

            Add(userToAdd);

            return _mapper.Map<UserForDetailed>(userToAdd);
        }

        public UserForDetailed UpdateUser(int id, UserForInsert user)
        {
            var userToUpdate = GetById(id);

            _mapper.Map(user, userToUpdate);

            if (user.Password != null) 
            {
                userToUpdate.Password = EncryptPassword(user.Password);            
            }
            userToUpdate.Role = _context.Roles.FirstOrDefault(r => string.Equals(r.Name, user.Role));
            userToUpdate.Faculty = _context.Faculties.Find(user.Faculty.Id);

            if(user.Photo != null)
            {
                var uploadResult = UploadPhoto(user.Photo);
                userToUpdate.PhotoUrl = uploadResult.Url.ToString();
                userToUpdate.PhotoPublicId = uploadResult.PublicId;
            }

            _context.SaveChanges();

            return _mapper.Map<UserForDetailed>(GetById(id));
        }

        public void UpdatePassword(int id, PasswordToUpdate password)
        {
            var userToUpdate = GetById(id);

            if(!string.Equals(userToUpdate.Password, EncryptPassword(password.OldPassword)))
                throw new BadRequestException("Mật khẩu cũ không đúng");

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

        private ImageUploadResult UploadPhoto(string content)
        {
            var uploadResult = new ImageUploadResult();

            using(var memoryStream = new MemoryStream(Convert.FromBase64String(content)))
            {
                var uploadParams = new ImageUploadParams()
                    {
                        File = new FileDescription($"{Guid.NewGuid()}.jpg", memoryStream),
                        Transformation = new Transformation()
                            .Width(500).Height(500).Crop("fill").Gravity("face")
                    };

                    uploadResult = _cloudinary.Upload(uploadParams);
            }

            return uploadResult;
        }
    }
}