using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace iAttendTFL_WebApp.Models
{
    // An event that a user attended and the requirement it fulfills
    public class AttendedEvent
    {
        public account account { get; set; }
        public account_attendance account_attendance { get; set; }
        public scan_event scan_event { get; set; }
        public event_requirement event_requirement { get; set; }
        public requirement requirement { get; set; }
    }
}
