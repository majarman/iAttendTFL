using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using iAttendTFL_WebApp.Models;

namespace iAttendTFL_WebApp.Data
{
    public class iAttendTFL_WebAppContext : DbContext
    {
        public iAttendTFL_WebAppContext (DbContextOptions<iAttendTFL_WebAppContext> options)
            : base(options)
        {
        }




        public DbSet<iAttendTFL_WebApp.Models.account> account { get; set; }

        public DbSet<iAttendTFL_WebApp.Models.barcode> barcode { get; set; }


    }
}
