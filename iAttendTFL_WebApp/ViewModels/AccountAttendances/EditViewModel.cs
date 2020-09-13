using System;

namespace iAttendTFL_WebApp.ViewModels.AccountAttendances
{
    public class EditViewModel
    {
        public int AccountId { get; set; }
        public int ScanEventId { get; set; }
        public bool IsValid { get; set; }
        public DateTime AttendanceTime { get; set; }
    }
}
