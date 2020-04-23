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
    public class ScanEventsController : Controller
    {
        private readonly iAttendTFL_WebAppContext _context;

        public ScanEventsController(iAttendTFL_WebAppContext context)
        {
            _context = context;
        }

        // GET: ScanEvents
        public async Task<IActionResult> Index()
        {
            return View(await _context.scan_event.ToListAsync());
        }

        // GET: ScanEvents/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var scan_event = await _context.scan_event
                .FirstOrDefaultAsync(m => m.id == id);
            if (scan_event == null)
            {
                return NotFound();
            }

            return View(scan_event);
        }

        // GET: ScanEvents/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: ScanEvents/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("id,name,start_time,end_time")] scan_event scan_event)
        {
            if (ModelState.IsValid)
            {
                _context.Add(scan_event);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(scan_event);
        }

        // GET: ScanEvents/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var scan_event = await _context.scan_event.FindAsync(id);
            if (scan_event == null)
            {
                return NotFound();
            }
            return View(scan_event);
        }

        // POST: ScanEvents/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("id,name,start_time,end_time")] scan_event scan_event)
        {
            if (id != scan_event.id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(scan_event);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!scan_eventExists(scan_event.id))
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
            return View(scan_event);
        }

        // GET: ScanEvents/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var scan_event = await _context.scan_event
                .FirstOrDefaultAsync(m => m.id == id);
            if (scan_event == null)
            {
                return NotFound();
            }

            return View(scan_event);
        }

        // POST: ScanEvents/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var scan_event = await _context.scan_event.FindAsync(id);
            _context.scan_event.Remove(scan_event);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool scan_eventExists(int id)
        {
            return _context.scan_event.Any(e => e.id == id);
        }
    }
}
