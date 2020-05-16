using System;

namespace DUTComputerLabs.API.Dtos
{
    public class BookingForDetailed
    {
        public int Id { get; set; }

        public string LabName { get; set; }

        public string BookerName { get; set; }

        public DateTime BookingDate { get; set; }

        public int StartAt { get; set; }

        public int EndAt { get; set; }

        public string Status { get; set; }

        public string Description { get; set; }
    }
}