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
    public class TokensController : Controller
    {
        private readonly iAttendTFL_WebAppContext _context;

        public TokensController(iAttendTFL_WebAppContext context)
        {
            _context = context;
        }

        // GET: Tokens
        public async Task<IActionResult> Index()
        {
            var iAttendTFL_WebAppContext = _context.token.Include(t => t.account);
            return View(await iAttendTFL_WebAppContext.ToListAsync());
        }

        // GET: Tokens/Details/5
        public async Task<IActionResult> Details(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var token = await _context.token
                .Include(t => t.account)
                .FirstOrDefaultAsync(m => m.token_hash == id);
            if (token == null)
            {
                return NotFound();
            }

            return View(token);
        }

        // GET: Tokens/Create
        public IActionResult Create()
        {
            ViewData["account_id"] = new SelectList(_context.account, "id", "id");
            return View();
        }

        // POST: Tokens/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("token_hash,salt,expiration_time,is_valid,type,account_id")] token token)
        {
            if (ModelState.IsValid)
            {
                _context.Add(token);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["account_id"] = new SelectList(_context.account, "id", "id", token.account_id);
            return View(token);
        }

        // GET: Tokens/Edit/5
        public async Task<IActionResult> Edit(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var token = await _context.token.FindAsync(id);
            if (token == null)
            {
                return NotFound();
            }
            ViewData["account_id"] = new SelectList(_context.account, "id", "id", token.account_id);
            return View(token);
        }

        // POST: Tokens/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(string id, [Bind("token_hash,salt,expiration_time,is_valid,type,account_id")] token token)
        {
            if (id != token.token_hash)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(token);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!tokenExists(token.token_hash))
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
            ViewData["account_id"] = new SelectList(_context.account, "id", "id", token.account_id);
            return View(token);
        }

        // GET: Tokens/Delete/5
        public async Task<IActionResult> Delete(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var token = await _context.token
                .Include(t => t.account)
                .FirstOrDefaultAsync(m => m.token_hash == id);
            if (token == null)
            {
                return NotFound();
            }

            return View(token);
        }

        // POST: Tokens/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(string id)
        {
            var token = await _context.token.FindAsync(id);
            _context.token.Remove(token);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool tokenExists(string id)
        {
            return _context.token.Any(e => e.token_hash == id);
        }
    }
}
