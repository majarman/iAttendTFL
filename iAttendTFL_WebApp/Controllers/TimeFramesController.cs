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
    public class TimeFramesController : Controller
    {
        private readonly iAttendTFL_WebAppContext _context;

        public TimeFramesController(iAttendTFL_WebAppContext context)
        {
            _context = context;
        }

        public int NewestTimeFrameID()
        {
            var id = _context.time_frame
                     .OrderByDescending(c => c.start_date)
                     .FirstOrDefault();
            
            if (id != null)
            {
                return id.id;
            }

            return int.MinValue;
        }

        public int CurrentTimeFrameID()
        {
            var id = _context.time_frame
                     .Where(c => c.start_date <= DateTime.Now && c.end_date >= DateTime.Now)
                     .FirstOrDefault();
            
            if (id != null)
            {
                return id.id;
            }

            return int.MinValue;
        }

        // GET: TimeFrames
        public async Task<IActionResult> Index()
        {
            return View(await _context.time_frame.ToListAsync());
        }

        // GET: TimeFrames/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var time_frame = await _context.time_frame
                .FirstOrDefaultAsync(m => m.id == id);
            if (time_frame == null)
            {
                return NotFound();
            }

            return View(time_frame);
        }

        // GET: TimeFrames/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: TimeFrames/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("name,start_date,end_date")] time_frame time_frame)
        {
            if (ModelState.IsValid)
            {
                _context.Add(time_frame);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(time_frame);
        }

        // GET: TimeFrames/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var time_frame = await _context.time_frame.FindAsync(id);
            if (time_frame == null)
            {
                return NotFound();
            }
            return View(time_frame);
        }

        // POST: TimeFrames/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("id,name,start_date,end_date")] time_frame time_frame)
        {
            if (id != time_frame.id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(time_frame);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!time_frameExists(time_frame.id))
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
            return View(time_frame);
        }

        // GET: TimeFrames/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var time_frame = await _context.time_frame
                .FirstOrDefaultAsync(m => m.id == id);
            if (time_frame == null)
            {
                return NotFound();
            }

            return View(time_frame);
        }

        // POST: TimeFrames/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var time_frame = await _context.time_frame.FindAsync(id);
            _context.time_frame.Remove(time_frame);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool time_frameExists(int id)
        {
            return _context.time_frame.Any(e => e.id == id);
        }
    }
}
