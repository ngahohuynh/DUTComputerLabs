using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace DUTComputerLabs.API.Models
{
    public class Booking
    {
        public int Id { get; set; }

        public int LabId { get; set; }

        [ForeignKey("LabId")]
        public virtual ComputerLab Lab { get; set; }

        public int UserId { get; set; }

        [ForeignKey("UserId")]
        public virtual User User { get; set; }

        public DateTime BookingDate { get; set; }

        public int StartAt { get; set; }

        public int EndAt { get; set; }

        public string Status { get; set; }

        public string Description { get; set; }

        public virtual ICollection<Notification> Notifications { get; set; }
        
        public virtual Feedback Feedback { get; set; }
    }
}