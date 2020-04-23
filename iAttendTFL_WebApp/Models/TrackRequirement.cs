using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace iAttendTFL_WebApp.Models
{
    public class track_requirement
    {
        public int track_id { get; set; }
        public int requirement_id { get; set; }
        public int num_required { get; set; }

        public virtual track track { get; set; }
        public virtual requirement requirement { get; set; }
    }
}
