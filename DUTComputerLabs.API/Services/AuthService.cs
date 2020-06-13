using System;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using AutoMapper;
using DUTComputerLabs.API.Data;
using DUTComputerLabs.API.Dtos;
using DUTComputerLabs.API.Exceptions;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;

namespace DUTComputerLabs.API.Services
{
    public class UserToken
    {
        public string Token { get; set; }

        public UserForDetailed User { get; set; }

        public UserToken(string token, UserForDetailed user)
        {
            Token = token;
            User = user;
        }
    }

    public interface IAuthService
    {
        UserToken Login(UserForLogin userForLogin);
    }

    public class AuthService : IAuthService
    {
        private readonly DataContext _context;
        private readonly IMapper _mapper;

        public AuthService(DataContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public UserToken Login(UserForLogin userForLogin)
        {
            var user = _context.Users.Include(u => u.Role).FirstOrDefault(u => string.Equals(u.Username, userForLogin.Username))
                ?? throw new BadRequestException("Người dùng không tồn tại");

            if(!VerifyPassword(userForLogin.Password, user.Password))
            {
                throw new BadRequestException("Sai mật khẩu. Vui lòng thử lại");
            }

            string token = GenerateToken(user.Id, user.Username, user.Role.Name);
            
            return new UserToken(token, _mapper.Map<UserForDetailed>(user));
        }

        private bool VerifyPassword(string password, string hashedPassword)
        {
            using (var md5 = new MD5CryptoServiceProvider())
            {
                var computedHash = Convert.ToBase64String(md5.ComputeHash(ASCIIEncoding.ASCII.GetBytes(password)));

                if (string.Equals(computedHash, hashedPassword))
                {
                    return true;
                }
            }
            return false;
        }

        private readonly string SecretKey = "super secret key";

        private string GenerateToken(int userId, string username, string role)
        {
            var claims = new[]
            {
                new Claim(ClaimTypes.NameIdentifier, userId.ToString()),
                new Claim(ClaimTypes.Name, username),
                new Claim(ClaimTypes.Role, role)
            };

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(SecretKey));

            var cred = new SigningCredentials(key, SecurityAlgorithms.HmacSha512Signature);

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(claims),
                Expires = DateTime.Now.AddDays(1),
                SigningCredentials = cred
            };

            var tokenHandler = new JwtSecurityTokenHandler();
            var token = tokenHandler.CreateToken(tokenDescriptor);

            return tokenHandler.WriteToken(token);
        }
    }
}