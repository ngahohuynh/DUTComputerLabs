using System;

namespace DUTComputerLabs.API.Dtos
{
    public class UserForDetailed
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public DateTime Birthday { get; set; }

        public string Gender { get; set; }

        public string Faculty { get; set; }

        public string PhoneNumber { get; set; }

        public string Email { get; set; }

        public string Address { get; set; }

        public string Username { get; set; }

        public string Role { get; set; }
    }
}