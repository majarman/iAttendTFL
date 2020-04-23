using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace iAttendTFL_WebApp.Models
{
    public class time_frame_track
    {
        public int track_id { get; set; }
        public int time_frame_id { get; set; }

        public virtual track track { get; set; }
        public virtual time_frame time_frame { get; set; }
    }
}
