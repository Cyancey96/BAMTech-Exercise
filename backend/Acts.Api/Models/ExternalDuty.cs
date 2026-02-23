using System;

namespace Acts.Api.Models
{
    public class ExternalDuty
    {
        public int DutyId { get; set; }

        public int PersonId { get; set; }

        public string Rank { get; set; } = string.Empty;

        public string Title { get; set; } = string.Empty;

        public DateTime StartDate { get; set; }

        public DateTime? EndDate { get; set; }

        public DateTime ImportedAt { get; set; }

        public DateTime UpdatedAt { get; set; }

        // Navigation
        public Person? Person { get; set; }
    }
}