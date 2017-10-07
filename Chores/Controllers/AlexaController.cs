using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Chores.Data;
using Chores.Models;

namespace Chores.Controllers
{
    [Produces("application/json")]
    [Route("api/Alexa")]
    public class AlexaController : Controller
    {
        private readonly ChoreContext _context;

        public AlexaController(ChoreContext context)
        {
            _context = context;
        }

        // GET: api/Alexa
        [HttpGet]
        public IEnumerable<Schedule> GetSchedules()
        {
            return _context.Schedules;
        }

        // GET: api/Alexa/5
        [HttpGet("{id}")]
        public async Task<IActionResult> GetSchedule([FromRoute] int id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var schedule = await _context.Schedules.SingleOrDefaultAsync(m => m.Id == id);

            if (schedule == null)
            {
                return NotFound();
            }

            return Ok(schedule);
        }

        public async Task<IActionResult> CompleteChore(string personName, string choreName)
        {
            var schedule = await _context.Schedules.SingleOrDefaultAsync(s => s.Chore.Name == choreName);
            if (schedule == null)
            {
                return NotFound();
            }
            var person = await _context.People.SingleOrDefaultAsync(p => p.Name == personName);
            if (person == null)
            {
                return NotFound();
            }
            schedule.PersonId = person.Id;
            schedule.CompletedDate = DateTime.Now;
            await _context.SaveChangesAsync();
            return NoContent();
        }

        public async Task<IActionResult> GetChoresForPerson(string personName)
        {
            return await GetChoresForPersonForFutureDays(personName, 0);
        }

        public async Task<IActionResult> GetChoresForDay()
        {
            return await GetChoresForFutureDays(0);
        }

        public async Task<IActionResult> GetChoresForFutureDays(int days)
        {
            if (days < 0)
            {
                return BadRequest();
            }
            var chores = _context.Schedules.Where(s => !s.CompletedDate.HasValue && s.TargetDate.Date <= DateTime.Now.AddDays(days).Date);
            return Ok(chores);
        }

        public async Task<IActionResult> GetChoresForPersonForFutureDays(string personName, int days)
        {
            if (days < 0)
            {
                return BadRequest();
            }
            var person = await _context.People.SingleOrDefaultAsync(p => p.Name == personName);
            if (person == null)
            {
                return NotFound();
            }
            var chores = _context.Schedules.Where(s => s.PersonId == person.Id && !s.CompletedDate.HasValue && s.TargetDate.Date <= DateTime.Now.AddDays(days).Date);
            return Ok(chores);
        }

        public async Task<IActionResult> GetCompletedChores()
        {
            return await GetCompletedChoresForPreviousDays(0);
        }

        public async Task<IActionResult> GetCompletedChoresForPerson(string personName)
        {
            return await GetCompletedChoresForPersonForPreviousDays(personName, 0);
        }

        public async Task<IActionResult> GetCompletedChoresForPersonForPreviousDays(string personName, int days)
        {
            if (days < 0)
            {
                return BadRequest();
            }
            var person = await _context.People.SingleOrDefaultAsync(p => p.Name == personName);
            if (person == null)
            {
                return NotFound();
            }
            var chores = _context.Schedules.Where(s => s.PersonId == person.Id && s.CompletedDate.HasValue && s.CompletedDate.Value.Date >= DateTime.Now.AddDays(0 - days).Date);
            return Ok(chores);
        }

        public async Task<IActionResult> GetCompletedChoresForPreviousDays(int days)
        {
            if (days < 0)
            {
                return BadRequest();
            }
            var chores = _context.Schedules.Where(s => s.CompletedDate.HasValue && s.CompletedDate.Value.Date >= DateTime.Now.AddDays(0 - days).Date);
            return Ok(chores);
        }

        // PUT: api/Alexa/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutSchedule([FromRoute] int id, [FromBody] Schedule schedule)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != schedule.Id)
            {
                return BadRequest();
            }

            _context.Entry(schedule).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ScheduleExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        // POST: api/Alexa
        [HttpPost]
        public async Task<IActionResult> PostSchedule([FromBody] Schedule schedule)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            _context.Schedules.Add(schedule);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetSchedule", new { id = schedule.Id }, schedule);
        }

        // DELETE: api/Alexa/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteSchedule([FromRoute] int id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var schedule = await _context.Schedules.SingleOrDefaultAsync(m => m.Id == id);
            if (schedule == null)
            {
                return NotFound();
            }

            _context.Schedules.Remove(schedule);
            await _context.SaveChangesAsync();

            return Ok(schedule);
        }

        private bool ScheduleExists(int id)
        {
            return _context.Schedules.Any(e => e.Id == id);
        }
    }
}