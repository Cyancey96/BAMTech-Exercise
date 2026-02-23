using Acts.Api.Data;
using Acts.Api.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Acts.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PersonController : ControllerBase
    {
        private readonly ActsDbContext _context;

        public PersonController(ActsDbContext context)
        {
            _context = context;
        }

        // GET: api/Person?id={id} or api/Person?name={name}
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Person>>> GetPerson([FromQuery] int? id, [FromQuery] string? name)
        {
            IQueryable<Person> query = _context.Person
                .Include(p => p.ExternalDuties)
                .Include(p => p.AstronautDuties);

            if (id.HasValue)
            {
                query = query.Where(p => p.PersonId == id.Value);
            }
            else if (!string.IsNullOrEmpty(name))
            {
                // Case-insensitive search by name
                query = query.Where(p => EF.Functions.Like(p.Name, name));
            }

            var persons = await query.ToListAsync();

            if (!persons.Any()) return NotFound();

            return persons;
        }

        // POST: api/Person
        [HttpPost]
        public async Task<ActionResult<Person>> PostPerson(Person person)
        {
            person.CreatedAt = DateTime.UtcNow;
            person.UpdatedAt = DateTime.UtcNow;

            _context.Person.Add(person);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetPerson), new { id = person.PersonId }, person);
        }

        // PUT: api/Person?id={id} or api/Person?name={name}
        [HttpPut]
        public async Task<IActionResult> PutPerson([FromQuery] int? id, [FromQuery] string? name, [FromBody] Person updatedPerson)
        {
            if (!id.HasValue && string.IsNullOrEmpty(name))
                return BadRequest("You must provide either id or name.");

            // Find existing person
            var person = await _context.Person
                .FirstOrDefaultAsync(p => (id.HasValue && p.PersonId == id.Value) ||
                                          (!string.IsNullOrEmpty(name) && p.Name == name));

            if (person == null) return NotFound();

            // Update fields
            person.Name = updatedPerson.Name;
            person.UpdatedAt = DateTime.UtcNow;

            await _context.SaveChangesAsync();
            return NoContent();
        }

        // DELETE: api/Person?id={id} or api/Person?name={name}
        [HttpDelete]
        public async Task<IActionResult> DeletePerson([FromQuery] int? id, [FromQuery] string? name)
        {
            if (!id.HasValue && string.IsNullOrEmpty(name))
                return BadRequest("You must provide either id or name.");

            var person = await _context.Person
                .FirstOrDefaultAsync(p => (id.HasValue && p.PersonId == id.Value) ||
                                          (!string.IsNullOrEmpty(name) && p.Name == name));

            if (person == null) return NotFound();

            _context.Person.Remove(person);
            await _context.SaveChangesAsync();
            return NoContent();
        }
    }
}