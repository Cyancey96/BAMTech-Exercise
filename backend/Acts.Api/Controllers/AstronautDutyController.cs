using Acts.Api.Data;
using Acts.Api.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Acts.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AstronautDutyController : ControllerBase
    {
        private readonly ActsDbContext _context;

        public AstronautDutyController(ActsDbContext context)
        {
            _context = context;
        }

        // GET: api/AstronautDuty?personName={name}&id={id}
        [HttpGet]
        public async Task<ActionResult<IEnumerable<AstronautDuty>>> GetAstronautDuty([FromQuery] int? id, [FromQuery] string? personName)
        {
            var query = _context.AstronautDuty
                .Include(d => d.Person)
                .Include(d => d.AstronautDetail)
                .AsQueryable();

            Console.WriteLine(personName);
            if (!string.IsNullOrEmpty(personName))
                query = query.Where(d => d.Person != null && d.Person.Name == personName);

            if (id.HasValue)
                query = query.Where(d => d.DutyId == id.Value);

            return await query.ToListAsync();
        }

        // POST: api/AstronautDuty
        [HttpPost]
        public async Task<ActionResult<AstronautDuty>> PostAstronautDuty([FromBody] AstronautDuty duty, [FromQuery] string? personName)
        {
            if (!string.IsNullOrEmpty(personName))
            {
                // Find person by name
                var person = await _context.Person.FirstOrDefaultAsync(p => p.Name == personName);
                if (person == null) return NotFound($"Person with name '{personName}' not found.");

                duty.PersonId = person.PersonId;
            }
            else if (duty.PersonId == 0)
            {
                return BadRequest("Either PersonId or personName must be provided.");
            }

            duty.CreatedAt = DateTime.UtcNow;
            duty.UpdatedAt = DateTime.UtcNow;

            _context.AstronautDuty.Add(duty);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetAstronautDuty), new { id = duty.DutyId }, duty);
        }

        // PUT: api/AstronautDuty/{id}
        [HttpPut("{id}")]
        public async Task<IActionResult> PutAstronautDuty(int id, AstronautDuty duty)
        {
            if (id != duty.DutyId) return BadRequest();

            var existing = await _context.AstronautDuty.FindAsync(id);
            if (existing == null) return NotFound();

            existing.Title = duty.Title;
            existing.Rank = duty.Rank;
            existing.StartDate = duty.StartDate;
            existing.EndDate = duty.EndDate;
            existing.UpdatedAt = DateTime.UtcNow;

            await _context.SaveChangesAsync();
            return NoContent();
        }

        // DELETE: api/AstronautDuty/{id}
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteAstronautDuty(int id)
        {
            var duty = await _context.AstronautDuty.FindAsync(id);
            if (duty == null) return NotFound();

            _context.AstronautDuty.Remove(duty);
            await _context.SaveChangesAsync();
            return NoContent();
        }
    }
}