using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace iAttendTFL_WebApp.Models
{
    // The attendance records of a user and the requirements they must fulfill
    public class StudentAttendance
    {
        public AttendedEvent attendedEvent { get; set; }
        public AccountRequirement accountRequirement { get; set; }
        public IQueryable<AttendedEvent> attendedEvents { get; set; }
        public IQueryable<AccountRequirement> accountRequirements { get; set; }
    }
}
