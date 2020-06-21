using System;

namespace DUTComputerLabs.API.Dtos
{
    public class NotificationForDetailed
    {
        public int Id { get; set; }

        public BookingForDetailed Booking { get; set; }

        public string Content { get; set; }

        public DateTime NoticeDate { get; set; }
    }
}