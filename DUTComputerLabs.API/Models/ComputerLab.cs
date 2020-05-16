using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace DUTComputerLabs.API.Models
{
    public class ComputerLab
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public string Condition { get; set; }

        public int Computers { get; set; }

        public int DamagedComputers { get; set; }

        public int Aircons { get; set; }

        public int OwnerId { get; set; }

        [ForeignKey("OwnerId")]
        public virtual User Owner { get; set; }

        public virtual ICollection<Booking> Bookings { get; set; }

        public virtual ICollection<Feedback> Feedbacks { get; set; }
    }
}