using System;

namespace Acts.Api.Models
{
    public class AstronautDuty
    {
        public int DutyId { get; set; }

        public int PersonId { get; set; }

        public string Rank { get; set; } = string.Empty;
        public string Title { get; set; } = string.Empty;

        public DateTime StartDate { get; set; }
        public DateTime? EndDate { get; set; }

        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }

        public Person? Person { get; set; }

        // 1-to-1 navigation
        public AstronautDetail? AstronautDetail { get; set; }
    }
}