using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace iAttendTFL_WebApp.Models
{
    public class track
    {
        public int id { get; set; }
        public String name { get; set; }
        public DateTime last_updated_date { get; set; }

        public virtual ICollection<track_requirement> track_requirements { get; set; }
        public virtual ICollection<time_frame_track> time_frame_tracks { get; set; }
    }
}
