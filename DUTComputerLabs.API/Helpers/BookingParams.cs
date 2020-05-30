using System;

namespace DUTComputerLabs.API.Helpers
{
    public class BookingParams : PaginationParams
    {
        public int? OwnerId { get; set; }

        public int? BookerId { get; set; }

        public DateTime BookingDate { get; set; }
    }
}