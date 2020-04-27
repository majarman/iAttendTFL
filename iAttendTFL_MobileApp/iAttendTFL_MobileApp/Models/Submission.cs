using System;
using System.Collections.Generic;
using System.Text;

namespace iAttendTFL_MobileApp.Models
{
    class Submission
    {
        public int id { get; set; }
        public string name { get; set; } = "New Event";
        public DateTime start_time { get; set; } = DateTime.UtcNow;
        public Submission(int id, string name)
        {
            this.id = id;
            this.name = name;
        }
    }
}
