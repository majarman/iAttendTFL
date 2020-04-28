using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace iAttendTFL_WebApp.Models
{
    // The public facing information that is displayed about an account
    public class AccountInfo
    {
        public int id { get; set; }
        public string first_name { get; set; }
        public string last_name { get; set; }
        public string email { get; set; }
        public char account_type { get; set; }
        public DateTime expected_graduation_date { get; set; }
        public string track { get; set; }
    }
}
