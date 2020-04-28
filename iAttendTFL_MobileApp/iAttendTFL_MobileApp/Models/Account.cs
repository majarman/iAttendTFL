using Microsoft.CodeAnalysis.CSharp.Syntax;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Threading.Tasks;
using BarcodeLib;
using static System.Net.Mime.MediaTypeNames;

namespace iAttendTFL_MobileApp.Models
{
    public class account
    {
        

        public int id { get; set; }
        public string first_name { get; set; }
        public string last_name { get; set; }
        public string email { get; set; }
        public string salt { get; set; } = "DEMOSALT";
        public string password_hash { get; set; }
        public char account_type { get; set; }
        public bool email_verified { get; set; } = false;
        public DateTime expected_graduation_date { get; set; }
        public int track_id { get; set; }
        public byte[] barcode { get; set; }
        public virtual ICollection<account_attendance> account_attendances { get; set; }
        //public virtual ICollection<token> tokens { get; set; }

    }
}