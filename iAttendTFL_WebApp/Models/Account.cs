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
        public string salt { get; set; } = "demosalt";
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
            return (byte[])imgCon.ConvertTo(img, typeof(byte[]));
        }

        public byte[] makeTheBarcode(int id)
        {
            String idString = id.ToString();
            //System.Diagnostics.Debug.WriteLine(idString);
            //TODO: validation?
            //System.Diagnostics.Debug.WriteLine("SOMETHING ELSE");
            //System.Diagnostics.Debug.WriteLine(StringToBarcodeImage(idString));
            byte[] myBarcode = ImageToByteArray(StringToBarcodeImage(idString));
            //System.Diagnostics.Debug.WriteLine(myBarcode);
            return myBarcode;
        }

        public void pushBarcode(int id)
        {
            System.Diagnostics.Debug.WriteLine("ID INCOMING");
            System.Diagnostics.Debug.WriteLine(id);
            System.Diagnostics.Debug.WriteLine("ID DONE");
            this.barcode = makeTheBarcode(id);
            System.Diagnostics.Debug.WriteLine("BARCODE INCOMING");
            System.Diagnostics.Debug.WriteLine(this.barcode);
            System.Diagnostics.Debug.WriteLine("BARCODE DONE");
        }

        

    }
}