using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Threading.Tasks;

namespace iAttendTFL_WebApp.Models
{
    public class barcode
    {
        private int id1;
        private byte[] v;

        public barcode(int id1, byte[] v)
        {
            this.id1 = id1;
            this.v = v;
        }

        public static int id { get; set; }
        public static Image barcodebytea { get; set; }
    }
}
