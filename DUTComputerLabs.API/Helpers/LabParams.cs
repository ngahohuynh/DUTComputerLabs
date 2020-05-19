using System;

namespace DUTComputerLabs.API.Helpers
{
    public class LabParams : PaginationParams
    {
        public int? OwnerId { get; set; }

        public DateTime BookingDate { get; set; }

        public int StartAt { get; set; } = 1;

        public int EndAt { get; set; } = 10;
    }
}