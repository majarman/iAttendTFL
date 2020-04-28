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
    public class TrackRequirementsController : Controller
    {
        private readonly iAttendTFL_WebAppContext _context;

        public TrackRequirementsController(iAttendTFL_WebAppContext context)
        {
            _context = context;
        }

        public int TrackRequirementCount(int track_id)
        {
            return (from tr in _context.track_requirement
                    where tr.track_id == track_id
                    group tr by tr.track_id into trc
                    select trc.Sum(x => x.num_required)).FirstOrDefault();
        }

        // GET: TrackRequirements
        public async Task<IActionResult> Index()
        {
            var iAttendTFL_WebAppContext = _context.track_requirement.Include(t => t.requirement).Include(t => t.track);
            return View(await iAttendTFL_WebAppContext.ToListAsync());
        }

        // GET: TrackRequirements/Details/5
        public async Task<IActionResult> Details(int? requirement_id, int? track_id)
        {
            if (requirement_id == null || track_id == null)
            {
                return NotFound();
            }

            var track_requirement = await _context.track_requirement
                .Include(t => t.requirement)
                .Include(t => t.track)
                .FirstOrDefaultAsync(m => m.requirement_id == requirement_id && m.track_id == track_id);
            if (track_requirement == null)
            {
                return NotFound();
            }

            return View(track_requirement);
        }

        // GET: TrackRequirements/Create
        public IActionResult Create()
        {
            ViewData["requirement_id"] = new SelectList(_context.requirement, "id", "id");
            ViewData["track_id"] = new SelectList(_context.track, "id", "id");
            return View();
        }

        // POST: TrackRequirements/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("track_id,requirement_id,num_required")] track_requirement track_requirement)
        {
            if (ModelState.IsValid)
            {
                _context.Add(track_requirement);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["requirement_id"] = new SelectList(_context.requirement, "id", "id", track_requirement.requirement_id);
            ViewData["track_id"] = new SelectList(_context.track, "id", "id", track_requirement.track_id);
            return View(track_requirement);
        }

        // GET: TrackRequirements/Edit/5
        public async Task<IActionResult> Edit(int? requirement_id, int? track_id)
        {
            if (requirement_id == null || track_id == null)
            {
                return NotFound();
            }

            var track_requirement = await _context.track_requirement.FindAsync(requirement_id, track_id);
            if (track_requirement == null)
            {
                return NotFound();
            }
            ViewData["requirement_id"] = new SelectList(_context.requirement, "id", "id", track_requirement.requirement_id);
            ViewData["track_id"] = new SelectList(_context.track, "id", "id", track_requirement.track_id);
            return View(track_requirement);
        }

        // POST: TrackRequirements/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int requirement_id, int track_id, [Bind("track_id,requirement_id,num_required")] track_requirement track_requirement)
        {
            if (requirement_id != track_requirement.requirement_id || track_id!= track_requirement.track_id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(track_requirement);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!track_requirementExists(track_requirement.requirement_id, track_requirement.track_id))
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
            ViewData["requirement_id"] = new SelectList(_context.requirement, "id", "id", track_requirement.requirement_id);
            ViewData["track_id"] = new SelectList(_context.track, "id", "id", track_requirement.track_id);
            return View(track_requirement);
        }

        // GET: TrackRequirements/Delete/5
        public async Task<IActionResult> Delete(int? requirement_id, int? track_id)
        {
            if (requirement_id == null || track_id == null)
            {
                return NotFound();
            }

            var track_requirement = await _context.track_requirement
                .Include(t => t.requirement)
                .Include(t => t.track)
                .FirstOrDefaultAsync(m => m.requirement_id == requirement_id && m.track_id == track_id);
            if (track_requirement == null)
            {
                return NotFound();
            }

            return View(track_requirement);
        }

        // POST: TrackRequirements/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int requirement_id, int track_id)
        {
            var track_requirement = await _context.track_requirement.FindAsync(requirement_id, track_id);
            _context.track_requirement.Remove(track_requirement);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool track_requirementExists(int requirement_id, int track_id)
        {
            return _context.track_requirement.Any(e => e.requirement_id == requirement_id && e.track_id == track_id);
        }
    }
}
