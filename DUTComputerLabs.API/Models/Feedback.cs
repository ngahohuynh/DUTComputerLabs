using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace DUTComputerLabs.API.Models
{
    public class Feedback
    {
        public int Id { get; set; }

        public int LabId { get; set; }

        [ForeignKey("LabId")]
        public virtual ComputerLab Lab { get; set; }

        public int UserId { get; set; }

        [ForeignKey("UserId")]
        public virtual User User { get; set; }

        public string Content { get; set; }

        public DateTime FeedbackDate { get; set; }
    }
}