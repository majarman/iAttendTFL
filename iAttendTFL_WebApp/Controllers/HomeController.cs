using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using iAttendTFL_WebApp.Models;
using System.Security.Cryptography;
using Microsoft.AspNetCore.Cryptography.KeyDerivation;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Session;

namespace iAttendTFL_WebApp.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }

        public IActionResult Account()
        {
            if (HttpContext.Session.GetString("Email") == null)
            {
                return RedirectToAction("NotLoggedIn");
            }
            else if (Char.ToLower(Convert.ToChar(HttpContext.Session.GetString("AccountType"))).Equals('a'))
            {
                return RedirectToAction("MyAdminAccount");
            }

            return RedirectToAction("MyAccount");
        }

        public IActionResult AlreadyLoggedIn()
        {
            if (HttpContext.Session.GetString("Email") == null)
            {
                return RedirectToAction("NotLoggedIn");
            }

            return View();
        }

        [HttpPost]
        public IActionResult AttemptLogin(string email, string password)
        {
            if (HttpContext.Session.GetString("Email") != null)
            {
                return RedirectToAction("AlreadyLoggedIn");
            }
            // else if (email is in database)
            // {
                char accountType = '~';             // GET ACCOUNT TYPE FROM DB
                byte[] hashedPassword = null;       // GET HASHED PASSWORD FROM DB
                byte[] salt = null;                 // GET SALT FROM DB

                byte[] hashedInput = KeyDerivation.Pbkdf2(
                    password: password.ToLower(),
                    salt: salt,
                    prf: KeyDerivationPrf.HMACSHA1,
                    iterationCount: 10000,
                    numBytesRequested: 256 / 8);

                if (hashedInput == hashedPassword)
                {
                    HttpContext.Session.SetString("Email", email);
                    HttpContext.Session.SetString("AccountType", Convert.ToString(accountType));

                    return RedirectToAction("Attendance");
                }
            // }

            return RedirectToAction("Login", new { error = true });
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

            return RedirectToAction("StudentAttendance");
        }

        public IActionResult ChangePassword()
        {
            if (HttpContext.Session.GetString("Email") != null)
            {
                foreach (var cookie in Request.Cookies.Keys)
                {
                    if (cookie == ".AspNetCore.Session")
                    {
                        Response.Cookies.Delete(cookie);
                    }
                }
            }

            return View();
        }

        public IActionResult CreateAccount()
        {
            return View();
        }

        public IActionResult DoesNotHavePermission(bool requiresMod)
        {
            if (HttpContext.Session.GetString("Email") == null)
            {
                return RedirectToAction("NotLoggedIn");
            }

            ViewData["RequiresMod"] = requiresMod;

            return View();
        }

        public IActionResult EditAnAccount()
        {
            if(HttpContext.Session.GetString("Email") == null)
            {
                return RedirectToAction("NotLoggedIn");
            }
            else if (!Char.ToLower(Convert.ToChar(HttpContext.Session.GetString("AccountType"))).Equals('a'))
            {
                return RedirectToAction("DoesNotHavePermission", new { requiresMod = false });
            }

            return View();
        }
        
        public IActionResult EditMyAdminAccount()
        {
            if (HttpContext.Session.GetString("Email") == null)
            {
                return RedirectToAction("NotLoggedIn");
            }
            else if (!Char.ToLower(Convert.ToChar(HttpContext.Session.GetString("AccountType"))).Equals('a'))
            {
                return RedirectToAction("DoesNotHavePermission", new { requiresMod = false });
            }

            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
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
                return RedirectToAction("DoesNotHavePermission", new { requiresMod = true });
            }

            return View();
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Login(bool error = false)
        {
            if (HttpContext.Session.GetString("Email") != null)
            {
                return RedirectToAction("AlreadyLoggedIn");
            }
            else if (error)
            {
                ViewData["Error"] = "Either the username or password was incorrect.";
            }

            return View();
        }

        public IActionResult Logout()
        {
            if (HttpContext.Session.GetString("Email") == null)
            {
                return RedirectToAction("NotLoggedIn");
            }

            foreach (var cookie in Request.Cookies.Keys)
            {
                if (cookie == ".AspNetCore.Session")
                {
                    Response.Cookies.Delete(cookie);
                }
            }

            return RedirectToAction("Index");
        }

        public IActionResult ManageAccounts()
        {
            if (HttpContext.Session.GetString("Email") == null)
            {
                return RedirectToAction("NotLoggedIn");
            }
            else if (!Char.ToLower(Convert.ToChar(HttpContext.Session.GetString("AccountType"))).Equals('a'))
            {
                return RedirectToAction("DoesNotHavePermission", new { requiresMod = false });
            }

            return View();
        }

        public IActionResult MyAccount()
        {
            if (HttpContext.Session.GetString("Email") == null)
            {
                return RedirectToAction("NotLoggedIn");
            }

            return View();
        }

        public IActionResult MyAdminAccount()
        {
            if (HttpContext.Session.GetString("Email") == null)
            {
                return RedirectToAction("NotLoggedIn");
            }
            else if (!Char.ToLower(Convert.ToChar(HttpContext.Session.GetString("AccountType"))).Equals('a'))
            {
                return RedirectToAction("DoesNotHavePermission", new { requiresMod = false });
            }

            return View();
        }

        public IActionResult NotLoggedIn()
        {
            if (HttpContext.Session.GetString("Email") != null)
            {
                return RedirectToAction("AlreadyLoggedIn");
            }

            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }
        
        public IActionResult RequestNewPassword()
        {
            if (HttpContext.Session.GetString("Email") != null)
            {
                foreach (var cookie in Request.Cookies.Keys)
                {
                    if (cookie == ".AspNetCore.Session")
                    {
                        Response.Cookies.Delete(cookie);
                    }
                }
            }

            return View();
        }

        public IActionResult StudentAttendance()
        {
            if (HttpContext.Session.GetString("Email") == null)
            {
                return RedirectToAction("NotLoggedIn");
            }

            return View();
        }

        [HttpPost]
        public IActionResult TestAttemptLogin(string email, string password)
        {
            if (HttpContext.Session.GetString("Email") != null)
            {
                return RedirectToAction("AlreadyLoggedIn");
            }

            string hashedInput = password;
            string hashedPassword = "Password";
            char accountType = 's';

            if (hashedInput == hashedPassword)
            {
                HttpContext.Session.SetString("Email", email);
                HttpContext.Session.SetString("AccountType", Convert.ToString(accountType));

                return RedirectToAction("Attendance");
            }

            return RedirectToAction("Login", new { error = true });
        }

        public IActionResult TransferAdmin()
        {
            if (HttpContext.Session.GetString("Email") == null)
            {
                return RedirectToAction("NotLoggedIn");
            }
            else if (!Char.ToLower(Convert.ToChar(HttpContext.Session.GetString("AccountType"))).Equals('a'))
            {
                return RedirectToAction("DoesNotHavePermission", new { requiresMod = false });
            }

            return View();
        }
    }
}
