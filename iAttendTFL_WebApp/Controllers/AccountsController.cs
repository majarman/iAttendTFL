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
using System.Security.Cryptography;
using Microsoft.AspNetCore.Cryptography.KeyDerivation;
using System.Text;

namespace iAttendTFL_WebApp.Controllers
{
    public class AccountsController : Controller
    {
        private readonly iAttendTFL_WebAppContext _context;

        public AccountsController(iAttendTFL_WebAppContext context)
        {
            _context = context;
        }

        [HttpPost]
        public IActionResult AttemptLogin(string email, string password)
        {
            if (HttpContext.Session.GetString("Email") != null)
            {
                return RedirectToAction("AlreadyLoggedIn", "Home");
            }

            account a1 = _context.account.SingleOrDefault(account => account.email == email);

            if (a1 != null)
            {
                char accountType = a1.account_type;

                string hashedPassword = a1.password_hash;
                /*
                byte[] hashedPassword = Encoding.ASCII.GetBytes(a1.password_hash);
                byte[] salt = Encoding.ASCII.GetBytes(a1.salt);

                byte[] hashedInput = KeyDerivation.Pbkdf2(
                    password: password.ToLower(),
                    salt: salt,
                    prf: KeyDerivationPrf.HMACSHA1,
                    iterationCount: 10000,
                    numBytesRequested: 256 / 8);
                */
                string hashedInput = password;

                if (hashedInput == hashedPassword)
                {
                    HttpContext.Session.SetString("Email", email);
                    HttpContext.Session.SetString("AccountType", Convert.ToString(accountType));

                    return RedirectToAction("Attendance", "AccountAttendances");
                }
            }

            return RedirectToAction("Login", "Home", new { error = true });
        }

        public string FullName(int? id = null, string email = null, bool lastThenFirst = false)
        {
            IQueryable<account> account;
            if (id == null && string.IsNullOrEmpty(email))
            {
                return null;
            }
            else if (id != null)
            {
                account = from a in _context.account
                    where a.id == id
                    select a;
            }
            else
            {
                account = from a in _context.account
                    where a.email.ToLower() == email.ToLower()
                    select a;
            }

            string fullName;

            if (lastThenFirst)
            {
                fullName = account.First().last_name + ", " + account.First().first_name;
            }
            else
            {
                fullName = account.First().first_name + " " + account.First().last_name;
            }

            return fullName;
        }

        // GET: accounts
        public async Task<IActionResult> Index()
        {
            return View(await _context.account.ToListAsync());
        }

        // GET: accounts/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var account = await _context.account
                .FirstOrDefaultAsync(m => m.id == id);
            if (account == null)
            {
                return NotFound();
            }

            return View(account);
        }

        // GET: accounts/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: accounts/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("id,first_name,last_name,email,salt,password_hash,account_type,email_verified,expected_graduation_date,track_id")] account account)
        {
            if (ModelState.IsValid)
            {
                _context.Add(account);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(account);
        }

        // GET: accounts/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var account = await _context.account.FindAsync(id);
            if (account == null)
            {
                return NotFound();
            }
            return View(account);
        }

        // POST: accounts/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("id,first_name,last_name,email,salt,password_hash,account_type,email_verified,expected_graduation_date,track_id")] account account)
        {
            if (id != account.id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(account);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!accountExists(account.id))
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
            return View(account);
        }

        // GET: accounts/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var account = await _context.account
                .FirstOrDefaultAsync(m => m.id == id);
            if (account == null)
            {
                return NotFound();
            }

            return View(account);
        }

        // POST: accounts/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var account = await _context.account.FindAsync(id);
            _context.account.Remove(account);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool accountExists(int id)
        {
            return _context.account.Any(e => e.id == id);
        }
    }
}
