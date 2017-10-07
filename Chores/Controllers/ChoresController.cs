using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Chores.Data;
using Chores.Models;

namespace Chores.Controllers
{
    public class ChoresController : Controller
    {
        private readonly ChoreContext _context;

        public ChoresController(ChoreContext context)
        {
            _context = context;    
        }

        // GET: Chores
        public async Task<IActionResult> Index()
        {
            var choreContext = _context.Chores.Include(c => c.Person);
            return View(await choreContext.ToListAsync());
        }

        // GET: Chores/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var chore = await _context.Chores
                .Include(c => c.Person)
                .SingleOrDefaultAsync(m => m.Id == id);
            if (chore == null)
            {
                return NotFound();
            }

            return View(chore);
        }

        // GET: Chores/Create
        public IActionResult Create()
        {
            ViewData["PersonId"] = new SelectList(_context.People, "Id", "Id");
            return View();
        }

        // POST: Chores/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Name,Description,PointValue,Frequency,AutoSchedule,PersonId")] Chore chore)
        {
            if (ModelState.IsValid)
            {
                _context.Add(chore);
                await _context.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            ViewData["PersonId"] = new SelectList(_context.People, "Id", "Id", chore.PersonId);
            return View(chore);
        }

        // GET: Chores/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var chore = await _context.Chores.SingleOrDefaultAsync(m => m.Id == id);
            if (chore == null)
            {
                return NotFound();
            }
            ViewData["PersonId"] = new SelectList(_context.People, "Id", "Id", chore.PersonId);
            return View(chore);
        }

        // POST: Chores/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Name,Description,PointValue,Frequency,AutoSchedule,PersonId")] Chore chore)
        {
            if (id != chore.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(chore);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ChoreExists(chore.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction("Index");
            }
            ViewData["PersonId"] = new SelectList(_context.People, "Id", "Id", chore.PersonId);
            return View(chore);
        }

        // GET: Chores/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var chore = await _context.Chores
                .Include(c => c.Person)
                .SingleOrDefaultAsync(m => m.Id == id);
            if (chore == null)
            {
                return NotFound();
            }

            return View(chore);
        }

        // POST: Chores/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var chore = await _context.Chores.SingleOrDefaultAsync(m => m.Id == id);
            _context.Chores.Remove(chore);
            await _context.SaveChangesAsync();
            return RedirectToAction("Index");
        }

        private bool ChoreExists(int id)
        {
            return _context.Chores.Any(e => e.Id == id);
        }
    }
}
