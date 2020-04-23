using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace iAttendTFL_WebApp.Models
{
    public class requirement
    {
        public int id { get; set; }
        public string name { get; set; }

        public virtual ICollection<track_requirement> track_requirements { get; set; }
        public virtual ICollection<event_requirement> event_requirements { get; set; }
    }
}
