using System;

namespace DUTComputerLabs.API.Dtos
{
    public class FeedbackForDetailed
    {
        public int Id { get; set; }

        public UserForList User { get; set; }

        public string Content { get; set; }

        public DateTime FeedbackDate { get; set; }
    }
}