using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace iAttendTFL_WebApp.Models
{
    public class Account
    {
#pragma warning disable IDE1006 // Naming Styles
        public int id { get; set; }
#pragma warning restore IDE1006 // Naming Styles
        public string first_name { get; set; }
#pragma warning restore IDE1006 // Naming Styles
        public string last_name { get; set; }
#pragma warning restore IDE1006 // Naming Styles
        public string email { get; set; }
#pragma warning restore IDE1006 // Naming Styles
        public string salt { get; set; }
#pragma warning restore IDE1006 // Naming Styles
        public string password_hash { get; set; }
#pragma warning restore IDE1006 // Naming Styles
        public char account_type { get; set; }
#pragma warning restore IDE1006 // Naming Styles
        public bool email_verified { get; set; }
#pragma warning restore IDE1006 // Naming Styles
        public DateTime expected_graduation_date { get; set; }
#pragma warning restore IDE1006 // Naming Styles
        public int track_id { get; set; }

    }
}
