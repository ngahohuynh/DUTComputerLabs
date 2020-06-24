using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace DUTComputerLabs.API.Models
{
    public class User
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public string PhotoUrl { get; set; }

        public string PhotoPublicId { get; set; }

        public DateTime Birthday { get; set; }

        public bool Gender { get; set; }

        public int FacultyId { get; set; }

        [ForeignKey("FacultyId")]
        public virtual Faculty Faculty { get; set; }

        public string PhoneNumber { get; set; }

        public string Email { get; set; }

        public string Address { get; set; }

        public string Username { get; set; }

        public string Password { get; set; }

        public int RoleId { get; set; }

        [ForeignKey("RoleId")]
        public virtual Role Role { get; set; }
    }
}