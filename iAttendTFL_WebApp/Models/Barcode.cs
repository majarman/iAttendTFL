using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Threading.Tasks;

namespace iAttendTFL_WebApp.Models
{
    public class barcode
    {
        
        public int id1 { get; set; }
        public byte[] v { get; set; }

        public barcode(int id1, byte[] v)
        {
            this.id1 = id1;
            this.v = v;
        }

    }
}
