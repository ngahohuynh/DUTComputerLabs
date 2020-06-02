using System;

namespace DUTComputerLabs.API.Dtos
{
    public class NotificationForDetailed
    {
        public int Id { get; set; }

        public string NoticeType { get; set; }

        public DateTime NoticeDate { get; set; }
    }
}