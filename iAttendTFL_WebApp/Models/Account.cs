using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace iAttendTFL_WebApp.Models
{
    public class account
    {
        public int id { get; set; }
        public string first_name { get; set; }
        public string last_name { get; set; }
        public string email { get; set; }
        public string salt { get; set; } = "demosalt";
        public string password_hash { get; set; }
        public char account_type { get; set; }
        public bool email_verified { get; set; } = false;
        public DateTime expected_graduation_date { get; set; }
        public int track_id { get; set; }
    }
}