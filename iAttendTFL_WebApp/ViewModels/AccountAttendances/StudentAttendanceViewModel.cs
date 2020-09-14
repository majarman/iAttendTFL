using iAttendTFL_WebApp.Models;
using System.Collections.Generic;

namespace iAttendTFL_WebApp.ViewModels.AccountAttendances
{
    public class StudentAttendanceViewModel
    {
        public List<time_frame> TimeFrames { get; set; }
        public int CurrentTimeFrameId { get; set; }
        public string Email { get; set; }
        public List<int> AttendancePoints { get; set; }
        public List<int> Progress { get; set; }
        public List<AccountRequirement> AccountRequirements { get; set; }
        public List<AttendedEvent> AttendedEvents { get; set; }
    }
}
