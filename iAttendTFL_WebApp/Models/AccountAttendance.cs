using System;

namespace iAttendTFL_WebApp.Models
{
    public class account_attendance
    {
        public int account_id { get; set; }
        public int scan_event_id { get; set; }
        public bool is_valid { get; set; }
        public DateTime attendance_time { get; set; }

        public virtual account account { get; set; }
        public virtual scan_event scan_event { get; set; }
    }
}
