using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace DUTComputerLabs.API.Models
{
    public class Notification
    {
        public int Id { get; set; }

        public int BookingId { get; set; }

        [ForeignKey("BookingId")]
        public virtual Booking Booking { get; set; }

        public string NoticeType { get; set; }

        public DateTime NoticeDate { get; set; }
    }
}