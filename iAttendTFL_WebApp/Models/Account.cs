using Microsoft.CodeAnalysis.CSharp.Syntax;
using System;
using System.Collections.Generic;
using System.Drawing;
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
        public string salt { get; set; } = "DEMOSALT";
        public string password_hash { get; set; }
        public char account_type { get; set; }
        public bool email_verified { get; set; } = false;
        public DateTime expected_graduation_date { get; set; }
        public int track_id { get; set; }
        public byte[] barcode { get; set; }

        // BARCODE GENERATION METHODS
        private Image StringToBarcodeImage(String input)
        {
            var barcodeMaker = new BarcodeLib.Barcode();
            Image myBarcode = barcodeMaker.Encode(BarcodeLib.TYPE.CODE39, input);
            return myBarcode;
        }

        private byte[] ImageToByteArray(Image img)
        {
            ImageConverter imgCon = new ImageConverter();
            byte[] myByteArray = (byte[])imgCon.ConvertTo(img, typeof(byte[]));
            return myByteArray;
        }

        public byte[] makeTheBarcode(int id)
        {
            String idString = id.ToString();
            //TODO: validation?
            byte[] myBarcode = ImageToByteArray(StringToBarcodeImage(idString));
            return myBarcode;
        }

        public void pushBarcode(int id)
        {
            this.barcode = makeTheBarcode(id);
        }

        // BARCODE DECODING



    }
}