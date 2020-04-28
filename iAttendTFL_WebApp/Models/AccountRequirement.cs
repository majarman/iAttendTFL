using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace iAttendTFL_WebApp.Models
{
    // The requirements a user must fulfill based on their track
    public class AccountRequirement
    {
        public account account { get; set; }
        public track track { get; set; }
        public track_requirement track_requirement { get; set; }
        public requirement requirement { get; set; }
    }
}
