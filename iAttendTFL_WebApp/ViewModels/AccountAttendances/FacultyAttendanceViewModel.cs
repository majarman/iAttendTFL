using iAttendTFL_WebApp.Models;
using System.Collections.Generic;

namespace iAttendTFL_WebApp.ViewModels.AccountAttendances
{
    public class FacultyAttendanceViewModel
    {
        public List<time_frame> TimeFrames { get; set; }
        public int CurrentTimeFrameId { get; set; }
        public List<string> Emails { get; set; }
        public List<string> FullNames { get; set; }
        public List<int> AttendancePoints { get; set; }
        public List<int> NeededAttendancePoints { get; set; }
        public List<int> Progress { get; set; }
    }
}
