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
using System.Drawing;
using System.IO;
using System.Drawing.Imaging;
using BarcodeLib;

namespace iAttendTFL_WebApp.Controllers
{
    public class accountsController : Controller
    {
        private readonly iAttendTFL_WebAppContext _context;

        public accountsController(iAttendTFL_WebAppContext context)
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

                    return RedirectToAction("Attendance", "Home");
                }
            }

            return RedirectToAction("Login", "Home", new { error = true });
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
            if (account.barcode == null)
            {
                account.pushBarcode(account.id);
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
        public async Task<IActionResult> Create([Bind("first_name,last_name,email,salt,password_hash,password_confirm,gradMonth,gradYear,account_type,track_id")] account account)
        {
                
            if (ModelState.IsValid)
            {
                //if (account.email.EndsWith("@mountunion.edu"))
                //{
                    if (!emailExists(account.email))
                    {
                        //if (meetsComplexityRequirement(account.password_hash))
                        //{
                            if (account.password_hash == account.password_confirm)
                            {
                                account.account_type = 's';
                                account.salt = "ab";
                                account.email_verified = true;
                                account.barcode = Encoding.UTF8.GetBytes("TEST");
                                account.expected_graduation_date = formatDate(account.gradMonth, account.gradYear);
                                //account.expected_graduation_date = new DateTime(2020, 05, 16);
                                //account.pushBarcode(123456);
                                _context.Add(account);
                                await _context.SaveChangesAsync();
                                return RedirectToAction(nameof(Index));
                            
                            } // passwords match
                            else { return RedirectToAction(nameof(Create)); }
                        
                        //} //complexity requirement
                        //else { return RedirectToAction(nameof(Create)); }
                    
                        //return RedirectToAction(nameof(Index));

                    } // email does not already exist in database
                    else { return RedirectToAction(nameof(Create)); }

                //return RedirectToAction(nameof(Index));
                //}
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

        //==========================
        //ustilized custom functions
        //==========================

        private bool accountExists(int id)
        {
            return _context.account.Any(e => e.id == id);
        }

        private bool emailExists(string userEmail)
        {
            return _context.account.Any(e => e.email == userEmail);
        }

        private bool meetsComplexityRequirement(string userPassword)
        {
            if (userPassword.Length > 7 && userPassword.Any(c => char.IsDigit(c)) && userPassword.Any(c => char.IsUpper(c)) && userPassword.Any(c => char.IsSymbol(c)))
            {
                return true;
            }
            else { return false; }
        }
        
        private DateTime formatDate(string userMonth, string userYear)
        {
            string dateString = userYear + "-" + userMonth + "-28";
            
            DateTime formattedEGD;
            formattedEGD = DateTime.Parse(dateString);
            //formattedEGD = DateTime.TryParseExact(dateString, format, null,
                               //System.Globalization.DateTimeStyles.AllowWhiteSpaces |
                               //System.Globalization.DateTimeStyles.AdjustToUniversal,
                               //out formattedEGD);           
            System.Diagnostics.Debug.WriteLine("====formattedEGD====");
            System.Diagnostics.Debug.WriteLine(formattedEGD.ToString());
            System.Diagnostics.Debug.WriteLine("====formattedEGD====");
            return formattedEGD.Date;
        }

    }
}
