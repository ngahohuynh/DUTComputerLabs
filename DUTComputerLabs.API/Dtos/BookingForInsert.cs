using System;

namespace DUTComputerLabs.API.Dtos
{
    public class BookingForInsert
    {
        public ComputerLabForList Lab { get; set; }

        public int? UserId { get; set; }

        public DateTime BookingDate { get; set; }

        public int StartAt { get; set; }

        public int EndAt { get; set; }

        public string Description { get; set; }
    }
}