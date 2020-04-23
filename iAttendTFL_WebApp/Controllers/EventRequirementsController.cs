using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using iAttendTFL_WebApp.Data;
using iAttendTFL_WebApp.Models;

namespace iAttendTFL_WebApp.Controllers
{
    public class EventRequirementsController : Controller
    {
        private readonly iAttendTFL_WebAppContext _context;

        public EventRequirementsController(iAttendTFL_WebAppContext context)
        {
            _context = context;
        }

        // GET: EventRequirements
        public async Task<IActionResult> Index()
        {
            var iAttendTFL_WebAppContext = _context.event_requirement.Include(e => e.requirement).Include(e => e.scan_event);
            return View(await iAttendTFL_WebAppContext.ToListAsync());
        }

        // GET: EventRequirements/Details/5
        public async Task<IActionResult> Details(int? requirement_id, int? scan_event_id)
        {
            if (requirement_id == null || scan_event_id == null)
            {
                return NotFound();
            }

            var event_requirement = await _context.event_requirement
                .Include(e => e.requirement)
                .Include(e => e.scan_event)
                .FirstOrDefaultAsync(m => m.requirement_id == requirement_id && m.scan_event_id == scan_event_id);
            if (event_requirement == null)
            {
                return NotFound();
            }

            return View(event_requirement);
        }

        // GET: EventRequirements/Create
        public IActionResult Create()
        {
            ViewData["requirement_id"] = new SelectList(_context.requirement, "id", "id");
            ViewData["scan_event_id"] = new SelectList(_context.scan_event, "id", "id");
            return View();
        }

        // POST: EventRequirements/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("scan_event_id,requirement_id,num_fulfilled")] event_requirement event_requirement)
        {
            if (ModelState.IsValid)
            {
                _context.Add(event_requirement);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["requirement_id"] = new SelectList(_context.requirement, "id", "id", event_requirement.requirement_id);
            ViewData["scan_event_id"] = new SelectList(_context.scan_event, "id", "id", event_requirement.scan_event_id);
            return View(event_requirement);
        }

        // GET: EventRequirements/Edit/5
        public async Task<IActionResult> Edit(int? requirement_id, int? scan_event_id)
        {
            if (requirement_id == null || scan_event_id == null)
            {
                return NotFound();
            }

            var event_requirement = await _context.event_requirement.FindAsync(requirement_id, scan_event_id);
            if (event_requirement == null)
            {
                return NotFound();
            }
            ViewData["requirement_id"] = new SelectList(_context.requirement, "id", "id", event_requirement.requirement_id);
            ViewData["scan_event_id"] = new SelectList(_context.scan_event, "id", "id", event_requirement.scan_event_id);
            return View(event_requirement);
        }

        // POST: EventRequirements/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int requirement_id, int scan_event_id, [Bind("scan_event_id,requirement_id,num_fulfilled")] event_requirement event_requirement)
        {
            if (requirement_id != event_requirement.requirement_id || scan_event_id != event_requirement.scan_event_id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(event_requirement);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!event_requirementExists(event_requirement.requirement_id, event_requirement.scan_event_id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            ViewData["requirement_id"] = new SelectList(_context.requirement, "id", "id", event_requirement.requirement_id);
            ViewData["scan_event_id"] = new SelectList(_context.scan_event, "id", "id", event_requirement.scan_event_id);
            return View(event_requirement);
        }

        // GET: EventRequirements/Delete/5
        public async Task<IActionResult> Delete(int? requirement_id, int? scan_event_id)
        {
            if (requirement_id == null || scan_event_id == null)
            {
                return NotFound();
            }

            var event_requirement = await _context.event_requirement
                .Include(e => e.requirement)
                .Include(e => e.scan_event)
                .FirstOrDefaultAsync(m => m.requirement_id == requirement_id && m.scan_event_id == scan_event_id);
            if (event_requirement == null)
            {
                return NotFound();
            }

            return View(event_requirement);
        }

        // POST: EventRequirements/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int requirement_id, int scan_event_id)
        {
            var event_requirement = await _context.event_requirement.FindAsync(requirement_id, scan_event_id);
            _context.event_requirement.Remove(event_requirement);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool event_requirementExists(int requirement_id, int scan_event_id)
        {
            return _context.event_requirement.Any(e => e.requirement_id == requirement_id && e.scan_event_id == scan_event_id);
        }
    }
}
