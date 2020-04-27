using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using iAttendTFL_WebApp.Data;
using iAttendTFL_WebApp.Models;
using Microsoft.AspNetCore.Http;
using System.Dynamic;

namespace iAttendTFL_WebApp.Controllers
{
    public class AccountAttendancesController : Controller
    {
        private readonly iAttendTFL_WebAppContext _context;

        public AccountAttendancesController(iAttendTFL_WebAppContext context)
        {
            _context = context;
        }

        public List<AttendedEvent> AttendedEvents(string email, DateTime time_frame_start, DateTime time_frame_end)
        {
            if (string.IsNullOrEmpty(email) || time_frame_start == null || time_frame_end == null)
            {
                return null;
            }

            return (from a in _context.account
                   join aa in _context.account_attendance
                       on a.id equals aa.account_id
                   join se in _context.scan_event
                      on aa.scan_event_id equals se.id
                   join er in _context.event_requirement
                      on se.id equals er.scan_event_id
                   join r in _context.requirement
                      on er.requirement_id equals r.id
                   where a.email.ToLower() == email.ToLower()
                      && aa.attendance_time >= time_frame_start
                      && aa.attendance_time <= time_frame_end
                   orderby aa.attendance_time descending, se.name, r.name
                   select new AttendedEvent
                   {
                       account = a,
                       account_attendance = aa,
                       scan_event = se,
                       event_requirement = er,
                       requirement = r
                   }).ToList();
        }

        public List<EventsFulfilled> EventsFulfilled(string email, DateTime time_frame_start, DateTime time_frame_end)
        {
            if (string.IsNullOrEmpty(email) || time_frame_start == null || time_frame_end == null)
            {
                return null;
            }

            return (from a in _context.account
                   join aa in _context.account_attendance
                      on a.id equals aa.account_id
                   join se in _context.scan_event
                     on aa.scan_event_id equals se.id
                   join er in _context.event_requirement
                     on se.id equals er.scan_event_id
                   join r in _context.requirement
                     on er.requirement_id equals r.id
                   where a.email.ToLower() == email.ToLower()
                     && aa.attendance_time >= time_frame_start
                     && aa.attendance_time <= time_frame_end
                   group er by er.requirement_id into rc
                   select new EventsFulfilled
                   {
                       id = rc.Key,
                       count = rc.Sum(x => x.num_fulfilled)
                   }).ToList();
        }

        public IActionResult Attendance()
        {
            if (HttpContext.Session.GetString("Email") == null)
            {
                return RedirectToAction("NotLoggedIn");
            }
            else if (Char.ToLower(Convert.ToChar(HttpContext.Session.GetString("AccountType"))).Equals('m') ||
                     Char.ToLower(Convert.ToChar(HttpContext.Session.GetString("AccountType"))).Equals('a'))
            {
                return RedirectToAction("FacultyAttendance");
            }

            return RedirectToAction("StudentAttendance", new { email = HttpContext.Session.GetString("Email") });
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

        public IActionResult FacultyAttendance()
        {
            if (HttpContext.Session.GetString("Email") == null)
            {
                return RedirectToAction("NotLoggedIn");
            }
            else if (!Char.ToLower(Convert.ToChar(HttpContext.Session.GetString("AccountType"))).Equals('a') &&
                     !Char.ToLower(Convert.ToChar(HttpContext.Session.GetString("AccountType"))).Equals('m'))
            {
                return RedirectToAction("DoesNotHavePermission", "Home", new { requiresMod = true });
            }

            return View();
        }

        public IActionResult StudentAttendance(string email, int? time_frame_id)
        {
            if (HttpContext.Session.GetString("Email") == null)
            {
                return RedirectToAction("NotLoggedIn", "Home");
            }
            else if (!Char.ToLower(Convert.ToChar(HttpContext.Session.GetString("AccountType"))).Equals('a')
                && !Char.ToLower(Convert.ToChar(HttpContext.Session.GetString("AccountType"))).Equals('m')
                && HttpContext.Session.GetString("Email") != email)
            {
                return NotFound();
            }
            else if (string.IsNullOrEmpty(email))
            {
                return NotFound();
            }

            time_frame time_frame;
            TimeFramesController tfc = new TimeFramesController(_context);
            if (time_frame_id == null)
            {
                int id = tfc.CurrentTimeFrameID();
                if (id != int.MinValue)
                {
                    time_frame = _context.time_frame
                                 .FirstOrDefault(m => m.id == id);
                }

                id = tfc.NewestTimeFrameID();
                if (id != int.MinValue)
                {
                    time_frame = _context.time_frame
                                 .FirstOrDefault(m => m.id == id);
                }
                else
                {
                    return NotFound();
                }
            }
            else
            {
                try
                {
                    time_frame = _context.time_frame
                             .FirstOrDefault(m => m.id == time_frame_id);
                }
                catch
                {
                    int id = tfc.NewestTimeFrameID();
                    if (id != int.MinValue)
                    {
                        time_frame = _context.time_frame
                                     .FirstOrDefault(m => m.id == id);
                    }
                    else
                    {
                        return NotFound();
                    }
                }
            }

            DateTime time_frame_start = time_frame.start_date;
            DateTime time_frame_end = time_frame.end_date;

            AccountsController ac = new AccountsController(_context);
            ViewData["FullName"] = ac.FullName(email: email);
            var accountRequirements = ac.AccountRequirements(email);

            var attendedEvents = AttendedEvents(email, time_frame_start, time_frame_end);
            var eventsFulfilled = EventsFulfilled(email, time_frame_start, time_frame_end);

            List<time_frame> timeFrames = _context.time_frame.OrderByDescending(c => c.start_date).ToList();
            
            List<int> attendancePoints = new List<int>();
            List<int> progress = new List<int>();

            foreach (var ar in accountRequirements)
            {
                bool hasValue = false;
                
                foreach (var ef in eventsFulfilled)
                {
                    if (ar.requirement.id == ef.id)
                    {
                        attendancePoints.Add(ef.count);
                        progress.Add(Convert.ToInt32(
                            Decimal.Round((Convert.ToDecimal(ef.count) / Convert.ToDecimal(ar.track_requirement.num_required)) * 100,
                            MidpointRounding.AwayFromZero)));
                        hasValue = true;
                        break;
                    }
                }

                if(hasValue == false)
                {
                    attendancePoints.Add(0);
                    progress.Add(0);
                }
            }

            ViewBag.TimeFrames = timeFrames;
            ViewBag.AttendedEvents = attendedEvents;
            ViewBag.AccountRequirements = accountRequirements;
            ViewBag.AttendancePoints = attendancePoints;
            ViewBag.Progress = progress;

            return View();
        }

        private bool account_attendanceExists(int account_id, int scan_event_id)
        {
            return _context.account_attendance.Any(e => e.account_id == account_id && e.scan_event_id == scan_event_id);
        }
    }
}