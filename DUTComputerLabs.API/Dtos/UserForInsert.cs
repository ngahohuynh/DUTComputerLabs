using System;
using DUTComputerLabs.API.Models;

namespace DUTComputerLabs.API.Dtos
{
    public class UserForInsert
    {
        public string Name { get; set; }

        public DateTime Birthday { get; set; }

        public bool Gender { get; set; }

        public Faculty Faculty { get; set; }

        public string PhoneNumber { get; set; }

        public string Email { get; set; }

        public string Address { get; set; }

        public string Username { get; set; }

        public string Password { get; set; }

        public string Role { get; set; }
    }
}