using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace iAttendTFL_WebApp.Models
{
    public class AccountsBarcode : Controller
    {
        // GET: /<controller>/
        public IActionResult Index()
        {
            return View();
        }
        public account Account { get; set; }
        public barcode Barcode { get; set; }
    }
}
