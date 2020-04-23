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
    public class AccountAttendancesController : Controller
    {
        private readonly iAttendTFL_WebAppContext _context;

        public AccountAttendancesController(iAttendTFL_WebAppContext context)
        {
            _context = context;
        }

        // GET: AccountAttendances
        public async Task<IActionResult> Index()
        {
            var iAttendTFL_WebAppContext = _context.account_attendance.Include(a => a.account).Include(a => a.scan_event);
            return View(await iAttendTFL_WebAppContext.ToListAsync());
        }

        // GET: AccountAttendances/Details/5
        public async Task<IActionResult> Details(int? account_id, int? scan_event_id)
        {
            if (account_id == null || scan_event_id == null)
            {
                return NotFound();
            }

            var account_attendance = await _context.account_attendance
                .Include(a => a.account)
                .Include(a => a.scan_event)
                .FirstOrDefaultAsync(m => m.account_id == account_id && m.scan_event_id == scan_event_id);
            if (account_attendance == null)
            {
                return NotFound();
            }

            return View(account_attendance);
        }

        // GET: AccountAttendances/Create
        public IActionResult Create()
        {
            ViewData["account_id"] = new SelectList(_context.account, "id", "id");
            ViewData["scan_event_id"] = new SelectList(_context.scan_event, "id", "id");
            return View();
        }

        // POST: AccountAttendances/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("account_id,scan_event_id,is_valid,attendance_time")] account_attendance account_attendance)
        {
            if (ModelState.IsValid)
            {
                _context.Add(account_attendance);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["account_id"] = new SelectList(_context.account, "id", "id", account_attendance.account_id);
            ViewData["scan_event_id"] = new SelectList(_context.scan_event, "id", "id", account_attendance.scan_event_id);
            return View(account_attendance);
        }

        // GET: AccountAttendances/Edit/5
        public async Task<IActionResult> Edit(int? account_id, int? scan_event_id)
        {
            if (account_id == null || scan_event_id == null)
            {
                return NotFound();
            }

            var account_attendance = await _context.account_attendance.FindAsync(account_id, scan_event_id);
            if (account_attendance == null)
            {
                return NotFound();
            }
            ViewData["account_id"] = new SelectList(_context.account, "id", "id", account_attendance.account_id);
            ViewData["scan_event_id"] = new SelectList(_context.scan_event, "id", "id", account_attendance.scan_event_id);
            return View(account_attendance);
        }

        // POST: AccountAttendances/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int account_id, int scan_event_id, [Bind("account_id,scan_event_id,is_valid,attendance_time")] account_attendance account_attendance)
        {
            if (account_id != account_attendance.account_id || scan_event_id != account_attendance.scan_event_id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(account_attendance);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!account_attendanceExists(account_attendance.account_id, account_attendance.scan_event_id))
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
            ViewData["account_id"] = new SelectList(_context.account, "id", "id", account_attendance.account_id);
            ViewData["scan_event_id"] = new SelectList(_context.scan_event, "id", "id", account_attendance.scan_event_id);
            return View(account_attendance);
        }

        // GET: AccountAttendances/Delete/5
        public async Task<IActionResult> Delete(int? account_id, int? scan_event_id)
        {
            if (account_id == null || scan_event_id == null)
            {
                return NotFound();
            }

            var account_attendance = await _context.account_attendance
                .Include(a => a.account)
                .Include(a => a.scan_event)
                .FirstOrDefaultAsync(m => m.account_id == account_id && m.scan_event_id == scan_event_id);
            if (account_attendance == null)
            {
                return NotFound();
            }

            return View(account_attendance);
        }

        // POST: AccountAttendances/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int account_id, int scan_event_id)
        {
            var account_attendance = await _context.account_attendance.FindAsync(account_id, scan_event_id);
            _context.account_attendance.Remove(account_attendance);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool account_attendanceExists(int account_id, int scan_event_id)
        {
            return _context.account_attendance.Any(e => e.account_id == account_id && e.scan_event_id == scan_event_id);
        }
    }
}
