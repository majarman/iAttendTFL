using Microsoft.AspNetCore.Mvc.Rendering;
using System;

namespace iAttendTFL_WebApp.ViewModels.AccountAttendances
{
    public class CreateViewModel
    {
        public SelectList Accounts { get; set; }
        public SelectList Events { get; set; }
        public int AccountId { get; set; }
        public int ScanEventId { get; set; }
        public bool IsValid { get; set; }
        public DateTime AttendanceTime { get; set; }
    }
}
