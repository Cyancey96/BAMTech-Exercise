using Acts.Api.Data;
using Acts.Api.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Acts.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AstronautDetailController : ControllerBase
    {
        private readonly ActsDbContext _context;

        public AstronautDetailController(ActsDbContext context)
        {
            _context = context;
        }

        // GET: api/AstronautDetail
        [HttpGet]
        public async Task<ActionResult<IEnumerable<AstronautDetail>>> GetAstronautDetail()
        {
            return await _context.AstronautDetail
                .Include(ad => ad.AstronautDuty)
                .ToListAsync();
        }

        // GET: api/AstronautDetail/{id}
        [HttpGet("{id}")]
        public async Task<ActionResult<AstronautDetail>> GetAstronautDetail(int id)
        {
            var detail = await _context.AstronautDetail
                .Include(ad => ad.AstronautDuty)
                .FirstOrDefaultAsync(ad => ad.DutyId == id);

            if (detail == null) return NotFound();
            return detail;
        }

        // POST: api/AstronautDetail
        [HttpPost]
        public async Task<ActionResult<AstronautDetail>> PostAstronautDetail(AstronautDetail detail)
        {
            detail.CreatedAt = DateTime.UtcNow;
            detail.UpdatedAt = DateTime.UtcNow;

            _context.AstronautDetail.Add(detail);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetAstronautDetail), new { id = detail.DutyId }, detail);
        }

        // PUT: api/AstronautDetail/{id}
        [HttpPut("{id}")]
        public async Task<IActionResult> PutAstronautDetail(int id, AstronautDetail detail)
        {
            if (id != detail.DutyId) return BadRequest();

            var existing = await _context.AstronautDetail.FindAsync(id);
            if (existing == null) return NotFound();

            existing.UpdatedAt = DateTime.UtcNow;
            await _context.SaveChangesAsync();

            return NoContent();
        }

        // DELETE: api/AstronautDetail/{id}
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteAstronautDetail(int id)
        {
            var detail = await _context.AstronautDetail.FindAsync(id);
            if (detail == null) return NotFound();

            _context.AstronautDetail.Remove(detail);
            await _context.SaveChangesAsync();
            return NoContent();
        }
    }
}