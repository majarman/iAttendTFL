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
using iAttendTFL_WebApp.ViewModels.Accounts;

namespace iAttendTFL_WebApp.Controllers
{
    public class AccountsController : Controller
    {
        private readonly iAttendTFL_WebAppContext _context;

        public AccountsController(iAttendTFL_WebAppContext context)
        {
            _context = context;
        }

        public AccountInfo AccountInfo(string email)
        {
            if (string.IsNullOrEmpty(email))
            {
                return null;
            }

            return (from a in _context.account
                    join t in _context.track
                        on a.track_id equals t.id
                    where a.email.ToLower() == email.ToLower()
                    select new AccountInfo
                    {
                        account = a,
                        track = t
                    }).FirstOrDefault();
        }

        public List<string> CurrentAccountsEmails(DateTime timeFrameStart)
        {
            if (timeFrameStart == null)
            {
                return null;
            }

            return (from a in _context.account
                    join t in _context.track
                        on a.track_id equals t.id
                    where a.expected_graduation_date >= timeFrameStart
                        && a.account_type != 'a'
                        && a.account_type != 'm'
                    orderby a.last_name
                    select a.email).ToList();
        }

        public List<AccountRequirement> AccountRequirements(string email)
        {
            if (string.IsNullOrEmpty(email))
            {
                return null;
            }

            return (from a in _context.account
                    join t in _context.track
                        on a.track_id equals t.id
                    join tr in _context.track_requirement
                        on t.id equals tr.track_id
                    join r in _context.requirement
                        on tr.requirement_id equals r.id
                    where a.email.ToLower() == email.ToLower()
                    orderby r.name
                    select new AccountRequirement
                    {
                        account = a,
                        track = t,
                        track_requirement = tr,
                        requirement = r
                    }).ToList();
        }

        public List<int> AccountRequirementIDs(string email)
        {
            if (string.IsNullOrEmpty(email))
            {
                return null;
            }

            return (from a in _context.account
                    join t in _context.track
                        on a.track_id equals t.id
                    join tr in _context.track_requirement
                        on t.id equals tr.track_id
                    join r in _context.requirement
                        on tr.requirement_id equals r.id
                    where a.email.ToLower() == email.ToLower()
                    select r.id).ToList();
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

                    return RedirectToAction("MyAccount", "Accounts");
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
            List<int> selectionYears = new List<int>();
            int currentYear = DateTime.Now.Year;
            
            for (int i = currentYear; i < currentYear + 7; i++)
            {
                selectionYears.Add(i);
            }

            List<track> tracks = _context.track.OrderBy(t => t.name).ToList();

            ViewBag.Years = selectionYears;
            ViewBag.Tracks = tracks;

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
                                return RedirectToAction("Index", "Home");
                            
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

        public IActionResult MyAccount()
        {
            if (HttpContext.Session.GetString("Email") == null)
            {
                return RedirectToAction("NotLoggedIn", "Home");
            }

            AccountInfo accountInfo = AccountInfo(HttpContext.Session.GetString("Email"));
            account a = accountInfo.account;

            Image barcodeImg = a.StringToBarcodeImage(a.id.ToString());
            byte[] barcodeByteA = a.ImageToByteArray(barcodeImg);

            MyAccountViewModel model = new MyAccountViewModel();

            model.Barcode = barcodeByteA;
            model.FullName = FullName(email: accountInfo.account.email);
            model.Email = accountInfo.account.email;
            model.AccountTypeString = accountInfo.account.account_type switch
            {
                'a' => "Administrator",
                'm' => "Moderator",
                _ => "Student"
            };
            model.Track = accountInfo.track.name;
            model.ExpectedGraduationDate = String.Format("{0:MMMM yyyy}", accountInfo.account.expected_graduation_date);
            model.AccountType = Convert.ToChar(HttpContext.Session.GetString("AccountType"));

            return View();
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
