using Microsoft.EntityFrameworkCore;
using iAttendTFL_WebApp.Models;

namespace iAttendTFL_WebApp.Data
{
    public class iAttendpgscontext : DbContext
    {
        public iAttendpgscontext(DbContextOptions<iAttendpgscontext> options)
            : base(options)
        {
        }

        public DbSet<Account> Account { get; set; }
    }
}