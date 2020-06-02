using System;

namespace DUTComputerLabs.API.Dtos
{
    public class NotificationForInsert
    {
        public int BookingId { get; set; }

        public string NoticeType { get; set; }

        public DateTime NoticeDate { get; set; } = DateTime.Now;
    }
}