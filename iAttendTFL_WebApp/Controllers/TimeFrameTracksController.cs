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
    public class TimeFrameTracksController : Controller
    {
        private readonly iAttendTFL_WebAppContext _context;

        public TimeFrameTracksController(iAttendTFL_WebAppContext context)
        {
            _context = context;
        }

        // GET: TimeFrameTracks
        public async Task<IActionResult> Index()
        {
            var iAttendTFL_WebAppContext = _context.time_frame_track.Include(t => t.time_frame).Include(t => t.track);
            return View(await iAttendTFL_WebAppContext.ToListAsync());
        }

        // GET: TimeFrameTracks/Details/5
        public async Task<IActionResult> Details(int? time_frame_id, int? track_id)
        {
            if (time_frame_id == null || track_id == null)
            {
                return NotFound();
            }

            var time_frame_track = await _context.time_frame_track
                .Include(t => t.time_frame)
                .Include(t => t.track)
                .FirstOrDefaultAsync(m => m.track_id == track_id && m.time_frame_id == time_frame_id);
            if (time_frame_track == null)
            {
                return NotFound();
            }

            return View(time_frame_track);
        }

        // GET: TimeFrameTracks/Create
        public IActionResult Create()
        {
            ViewData["time_frame_id"] = new SelectList(_context.time_frame, "id", "id");
            ViewData["track_id"] = new SelectList(_context.track, "id", "id");
            return View();
        }

        // POST: TimeFrameTracks/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("track_id,time_frame_id")] time_frame_track time_frame_track)
        {
            if (ModelState.IsValid)
            {
                _context.Add(time_frame_track);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["time_frame_id"] = new SelectList(_context.time_frame, "id", "id", time_frame_track.time_frame_id);
            ViewData["track_id"] = new SelectList(_context.track, "id", "id", time_frame_track.track_id);
            return View(time_frame_track);
        }

        // GET: TimeFrameTracks/Edit/5
        public async Task<IActionResult> Edit(int? time_frame_id, int? track_id)
        {
            if (time_frame_id == null || track_id == null)
            {
                return NotFound();
            }

            var time_frame_track = await _context.time_frame_track.FindAsync(time_frame_id, track_id);
            if (time_frame_track == null)
            {
                return NotFound();
            }
            ViewData["time_frame_id"] = new SelectList(_context.time_frame, "id", "id", time_frame_track.time_frame_id);
            ViewData["track_id"] = new SelectList(_context.track, "id", "id", time_frame_track.track_id);
            return View(time_frame_track);
        }

        // POST: TimeFrameTracks/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int time_frame_id, int track_id, [Bind("track_id,time_frame_id")] time_frame_track time_frame_track)
        {
            if (time_frame_id != time_frame_track.time_frame_id || track_id != time_frame_track.track_id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(time_frame_track);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!time_frame_trackExists(time_frame_track.time_frame_id, time_frame_track.track_id))
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
            ViewData["time_frame_id"] = new SelectList(_context.time_frame, "id", "id", time_frame_track.time_frame_id);
            ViewData["track_id"] = new SelectList(_context.track, "id", "id", time_frame_track.track_id);
            return View(time_frame_track);
        }

        // GET: TimeFrameTracks/Delete/5
        public async Task<IActionResult> Delete(int? time_frame_id, int? track_id)
        {
            if (time_frame_id == null || track_id == null)
            {
                return NotFound();
            }

            var time_frame_track = await _context.time_frame_track
                .Include(t => t.time_frame)
                .Include(t => t.track)
                .FirstOrDefaultAsync(m => m.time_frame_id == time_frame_id && m.track_id == track_id);
            if (time_frame_track == null)
            {
                return NotFound();
            }

            return View(time_frame_track);
        }

        // POST: TimeFrameTracks/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int time_frame_id, int track_id)
        {
            var time_frame_track = await _context.time_frame_track.FindAsync(time_frame_id, track_id);
            _context.time_frame_track.Remove(time_frame_track);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool time_frame_trackExists(int time_frame_id, int track_id)
        {
            return _context.time_frame_track.Any(e => e.time_frame_id == time_frame_id && e.track_id == track_id);
        }
    }
}
