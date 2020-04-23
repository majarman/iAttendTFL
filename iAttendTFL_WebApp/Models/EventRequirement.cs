using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace iAttendTFL_WebApp.Models
{
    public class event_requirement
    {
        public int scan_event_id { get; set; }
        public int requirement_id { get; set; }
        public int num_fulfilled { get; set; }

        public virtual requirement requirement { get; set; }
        public virtual scan_event scan_event { get; set; }
    }
}
