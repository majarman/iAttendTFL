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
        public async Task<IActionResult> Create([Bind("id,first_name,last_name,email,salt,password_hash,account_type,email_verified,expected_graduation_date,track_id")] account account)
        {
            if (ModelState.IsValid)
            {
                //account.pushBarcode(123456);
                _context.Add(account);
                await _context.SaveChangesAsync();
                
                
                return RedirectToAction(nameof(Index));
            }
            
            return View(account);
        }

        // POST: accounts/CreateAccount    --Matthew Dutt Version--
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CreateAccount( string inputFirstName, string inputLastName, string inputEmail, string inputPassword, string inputConfirmPassword, string selectMonth, string selectYear, int selectTrack)
        {

            System.Diagnostics.Debug.WriteLine("Hello world");
            if (inputEmail.EndsWith("@mountunion.edu"))
            {
                if (!emailExists(inputEmail))
                {
                    if (meetsComplexityRequirement(inputPassword))
                    {
                        if (inputPassword == inputConfirmPassword)
                        {
                            DateTime gradDate = formatDate(selectMonth, selectYear); //takes string values from dropdown list selections, combines them, and creates a DateTime variable with that data.
                            account tempAccount = new account();
                            //add id increment handling stuff in web app in the future instead of db
                            tempAccount.id = 3001;
                            tempAccount.first_name = inputFirstName;
                            tempAccount.last_name = inputLastName;
                            tempAccount.email = inputEmail;
                            tempAccount.password_hash = inputPassword;
                            tempAccount.expected_graduation_date = gradDate;
                            tempAccount.track_id = selectTrack;
                            tempAccount.salt = "DefaultSalt";
                                if (ModelState.IsValid)
                                {
                                    _context.Add(tempAccount);
                                    _context.SaveChangesAsync();
                                    return RedirectToAction(nameof(Index));
                                }
                                return View(tempAccount);
                                //return view();
                            
                        } return NotFound(); //password match
                    } return NotFound(); //password meets complexity requirements
                } return NotFound(); //email does not exist
            } return NotFound(); //email suffix is @mountunion.edu

        } //

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
            if ( userPassword.Length > 7 && userPassword.Any(char.IsDigit) && userPassword.Any(char.IsUpper) && userPassword.Any(char.IsSymbol) )
            {
                return true;
            } else { return false; }
        }
        
        private DateTime formatDate(string userMonth, string userYear)
        {
            string format = "yyyyMMdd";
            string dateString = userYear + userMonth + "28";
            
            DateTime formattedEGD;
            formattedEGD = DateTime.Parse(dateString);
            //formattedEGD = DateTime.TryParseExact(dateString, format, null,
                               //System.Globalization.DateTimeStyles.AllowWhiteSpaces |
                               //System.Globalization.DateTimeStyles.AdjustToUniversal,
                               //out formattedEGD);           
            System.Diagnostics.Debug.WriteLine("====formattedEGD====");
            System.Diagnostics.Debug.WriteLine(formattedEGD.ToString());
            System.Diagnostics.Debug.WriteLine("====formattedEGD====");
            return formattedEGD;
        }

    }
}
