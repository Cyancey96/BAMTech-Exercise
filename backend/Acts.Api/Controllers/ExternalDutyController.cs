using Acts.Api.Data;
using Acts.Api.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Acts.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ExternalDutyController : ControllerBase
    {
        private readonly ActsDbContext _context;

        public ExternalDutyController(ActsDbContext context)
        {
            _context = context;
        }

        // GET: api/ExternalDuty
        [HttpGet]
        public async Task<ActionResult<IEnumerable<ExternalDuty>>> GetExternalDuty()
        {
            return await _context.ExternalDuty.Include(d => d.Person).ToListAsync();
        }

        // GET: api/ExternalDuty/{id}
        [HttpGet("{id}")]
        public async Task<ActionResult<ExternalDuty>> GetExternalDuty(int id)
        {
            var duty = await _context.ExternalDuty.Include(d => d.Person)
                .FirstOrDefaultAsync(d => d.DutyId == id);

            if (duty == null) return NotFound();
            return duty;
        }

        // POST: api/ExternalDuty
        [HttpPost]
        public async Task<ActionResult<ExternalDuty>> PostExternalDuty(ExternalDuty duty)
        {
            duty.ImportedAt = DateTime.UtcNow;
            duty.UpdatedAt = DateTime.UtcNow;

            _context.ExternalDuty.Add(duty);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetExternalDuty), new { id = duty.DutyId }, duty);
        }

        // PUT: api/ExternalDuty/{id}
        [HttpPut("{id}")]
        public async Task<IActionResult> PutExternalDuty(int id, ExternalDuty duty)
        {
            if (id != duty.DutyId) return BadRequest();

            var existing = await _context.ExternalDuty.FindAsync(id);
            if (existing == null) return NotFound();

            existing.Title = duty.Title;
            existing.Rank = duty.Rank;
            existing.StartDate = duty.StartDate;
            existing.EndDate = duty.EndDate;
            existing.UpdatedAt = DateTime.UtcNow;

            await _context.SaveChangesAsync();
            return NoContent();
        }

        // DELETE: api/ExternalDuty/{id}
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteExternalDuty(int id)
        {
            var duty = await _context.ExternalDuty.FindAsync(id);
            if (duty == null) return NotFound();

            _context.ExternalDuty.Remove(duty);
            await _context.SaveChangesAsync();
            return NoContent();
        }
    }
}