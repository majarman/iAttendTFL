using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace iAttendTFL_WebApp.Models
{
    public class Account
    {
        public int Id { get; set; }
        public string First_Name { get; set; }
        public string Last_Name { get; set; }
        public string Email { get; set; }
        public string Salt { get; set; }
        public string Password_Hash { get; set; }
        public char Account_Type { get; set; }
        public bool Email_Verified { get; set; }
        public DateTime Expected_Graduation_Date { get; set; }
        public int Track_Id { get; set; }
    }
}
