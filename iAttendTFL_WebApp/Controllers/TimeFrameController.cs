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
    public class TimeFrameController : Controller
    {
        private readonly iAttendTFL_WebAppContext _context;

        public TimeFrameController(iAttendTFL_WebAppContext context)
        {
            _context = context;
        }

        // GET: TimeFrame
        public async Task<IActionResult> Index()
        {
            return View(await _context.timeFrame.ToListAsync());
        }

        // GET: TimeFrame/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var timeFrame = await _context.timeFrame
                .FirstOrDefaultAsync(m => m.id == id);
            if (timeFrame == null)
            {
                return NotFound();
            }

            return View(timeFrame);
        }

        // GET: TimeFrame/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: TimeFrame/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("id,name,start_date,end_date")] timeFrame timeFrame)
        {
            if (ModelState.IsValid)
            {
                _context.Add(timeFrame);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(timeFrame);
        }

        // GET: TimeFrame/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var timeFrame = await _context.timeFrame.FindAsync(id);
            if (timeFrame == null)
            {
                return NotFound();
            }
            return View(timeFrame);
        }

        // POST: TimeFrame/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("id,name,start_date,end_date")] timeFrame timeFrame)
        {
            if (id != timeFrame.id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(timeFrame);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!timeFrameExists(timeFrame.id))
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
            return View(timeFrame);
        }

        // GET: TimeFrame/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var timeFrame = await _context.timeFrame
                .FirstOrDefaultAsync(m => m.id == id);
            if (timeFrame == null)
            {
                return NotFound();
            }

            return View(timeFrame);
        }

        // POST: TimeFrame/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var timeFrame = await _context.timeFrame.FindAsync(id);
            _context.timeFrame.Remove(timeFrame);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool timeFrameExists(int id)
        {
            return _context.timeFrame.Any(e => e.id == id);
        }
    }
}
