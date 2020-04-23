using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace iAttendTFL_WebApp.Models
{
    public class time_frame
    {
        public int id { get; set; }
        public string name { get; set; }
        public DateTime start_date { get; set; }
        public DateTime end_date { get; set; }

        public virtual ICollection<time_frame_track> time_frame_tracks { get; set; }
    }
}
