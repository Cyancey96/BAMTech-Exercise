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

        // PUT: api/AstronautDuty?id={id} or api/AstronautDuty?personName={name}
        [HttpPut]
        public async Task<IActionResult> PutAstronautDuty([FromQuery] int? id, [FromQuery] string? personName, [FromBody] AstronautDuty duty)
        {
            if (!id.HasValue && string.IsNullOrEmpty(personName))
                return BadRequest("Either id or personName must be provided.");

            var query = _context.AstronautDuty
                .Include(d => d.Person)
                .AsQueryable();

            if (!string.IsNullOrEmpty(personName))
                query = query.Where(d => d.Person != null && d.Person.Name == personName);

            if (id.HasValue)
                query = query.Where(d => d.DutyId == id.Value);

            var existing = await query.FirstOrDefaultAsync();
            if (existing == null) return NotFound();

            existing.Title = duty.Title;
            existing.Rank = duty.Rank;
            existing.StartDate = duty.StartDate;
            existing.EndDate = duty.EndDate;
            existing.UpdatedAt = DateTime.UtcNow;

            await _context.SaveChangesAsync();
            return NoContent();
        }

        // DELETE: api/AstronautDuty?id={id} or api/AstronautDuty?personName={name}
        [HttpDelete]
        public async Task<IActionResult> DeleteAstronautDuty([FromQuery] int? id, [FromQuery] string? personName)
        {
            if (!id.HasValue && string.IsNullOrEmpty(personName))
                return BadRequest("Either id or personName must be provided.");

            var query = _context.AstronautDuty
                .Include(d => d.Person)
                .AsQueryable();

            if (!string.IsNullOrEmpty(personName))
                query = query.Where(d => d.Person != null && d.Person.Name == personName);

            if (id.HasValue)
                query = query.Where(d => d.DutyId == id.Value);

            var duties = await query.ToListAsync();
            if (!duties.Any()) return NotFound();

            _context.AstronautDuty.RemoveRange(duties);
            await _context.SaveChangesAsync();
            return NoContent();
        }
    }
}