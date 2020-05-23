using System;

namespace DUTComputerLabs.API.Helpers
{
    public class LabParams : PaginationParams
    {
        public int? OwnerId { get; set; }

        public DateTime BookingDate { get; set; } = DateTime.Now;

        public int StartAt { get; set; } = 11;

        public int EndAt { get; set; } = 11;

        public int EditMode { get; set; }
    }
}