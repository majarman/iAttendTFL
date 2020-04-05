using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using iAttendTFL_WebApp.Models;

namespace iAttendTFL_WebApp.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
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
        
        public IActionResult Attendance(char accountType)
        {
            if (Char.ToLower(accountType).Equals('m') || Char.ToLower(accountType).Equals('a'))
            {
                return RedirectToAction("FacultyAttendance");
            }
            else
            {
                return RedirectToAction("StudentAttendance");
            }
        }
        
        public IActionResult Index()
        {
            return View();
        }
        
        public IActionResult Login()
        {
            return View();
        }
        
        public IActionResult ManageAccounts()
        {
            return View();
        }
        
        public IActionResult Account(char accountType)
        {
            if (Char.ToLower(accountType).Equals('a'))
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
