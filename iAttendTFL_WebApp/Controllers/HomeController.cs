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

        [HttpPost]
        public IActionResult AttemptLogin(string email, string password)
        {
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

            return RedirectToAction("Login", new { error = true });
        }

        [HttpPost]
        public IActionResult TestAttemptLogin(string email, string password)
        {
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

        public IActionResult ChangePassword()
        {
            return View();
        }

        public IActionResult CreateAccount()
        {
            return View();
        }
        
        public IActionResult EditAnAccount()
        {
            return View();
        }
        
        public IActionResult EditMyAdminAccount()
        {
            return View();
        }
        
        public IActionResult Attendance()
        {
            if (Char.ToLower(Convert.ToChar(HttpContext.Session.GetString("AccountType"))).Equals('m') ||
                Char.ToLower(Convert.ToChar(HttpContext.Session.GetString("AccountType"))).Equals('a'))
            {
                return RedirectToAction("FacultyAttendance");
            }
            else
            {
                return RedirectToAction("StudentAttendance");
            }
        }

        public IActionResult FacultyAttendance()
        {
            return View();
        }

        public IActionResult StudentAttendance()
        {
            return View();
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Login(bool error = false)
        {
            if (error)
            {
                ViewData["Error"] = "Either the username or password was incorrect";
            }
            return View();
        }

        public IActionResult ManageAccounts()
        {
            return View();
        }

        public IActionResult MyAccount()
        {
            return View();
        }

        public IActionResult MyAdminAccount()
        {
            return View();
        }

        public IActionResult Account()
        {
            if (Char.ToLower(Convert.ToChar(HttpContext.Session.GetString("AccountType"))).Equals('a'))
            {
                return RedirectToAction("MyAdminAccount");
            }
            else
            {
                return RedirectToAction("MyAccount");
            }
        }

        public IActionResult Privacy()
        {
            return View();
        }
        
        public IActionResult RequestNewPassword()
        {
            return View();
        }

        public IActionResult TransferAdmin()
        {
            return View();
        }
       

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
