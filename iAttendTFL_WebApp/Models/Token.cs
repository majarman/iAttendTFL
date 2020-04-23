using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace iAttendTFL_WebApp.Models
{
    public class token
    {
        public String token_hash { get; set; }
        public String salt { get; set; }
        public DateTime expiration_time { get; set; }
        public bool is_valid { get; set; }
        public Char type { get; set; }
        public int account_id { get; set; }

        public virtual account account { get; set; }
    }
}
