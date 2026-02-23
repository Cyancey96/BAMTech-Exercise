using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace Acts.Api.Models
{

public class Person
    {
        public int PersonId { get; set; }
        [Required]
        [MaxLength(200)]
        public string Name { get; set; } = string.Empty;
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
        [JsonIgnore]
        public ICollection<ExternalDuty> ExternalDuties { get; set; } = new List<ExternalDuty>();
        [JsonIgnore]
        public ICollection<AstronautDuty> AstronautDuties { get; set; } = new List<AstronautDuty>();
    }
}