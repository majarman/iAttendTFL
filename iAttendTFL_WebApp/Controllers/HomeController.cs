using System;
using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using iAttendTFL_WebApp.Models;
using Microsoft.AspNetCore.Http;
using BarcodeLib;
using System.Drawing;
using System.IO;
using Glimpse.AspNet.Tab;

namespace iAttendTFL_WebApp.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }

        public IActionResult AdminActions()
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

        public IActionResult AlreadyLoggedIn()
        {
            if (HttpContext.Session.GetString("Email") == null)
            {
                return RedirectToAction("NotLoggedIn");
            }

            return View();
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

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

        public IActionResult Events()
        {
            if (HttpContext.Session.GetString("Email") == null)
            {
                return RedirectToAction("NotLoggedIn");
            }

            ViewData["AccountType"] = HttpContext.Session.GetString("AccountType");

            return View();
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

        public IActionResult ManageRequirements()
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

        public IActionResult ManageTimeFrames()
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

        public IActionResult ManageTracks()
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

            ViewData["AccountType"] = HttpContext.Session.GetString("AccountType");

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

        public IActionResult TimeFrameTracks()
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

        public IActionResult TrackRequirements()
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
