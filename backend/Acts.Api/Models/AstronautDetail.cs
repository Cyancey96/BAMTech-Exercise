using System;

namespace Acts.Api.Models
{
    public class AstronautDetail
    {
        public int DutyId { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
        public AstronautDuty? AstronautDuty { get; set; }
    }
}