using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace iAttendTFL_MobileApp.Models
{
    public class scan_event
    {
        public int id { get; set; }
        public string name { get; set; }
        public DateTime start_time { get; set; }
        public DateTime? end_time { get; set; }

        public virtual ICollection<account_attendance> account_attendances { get; set; }
        //public virtual ICollection<event_requirement> event_requirements { get; set; }
    }
}
