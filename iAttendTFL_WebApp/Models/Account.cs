using Microsoft.CodeAnalysis.CSharp.Syntax;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Threading.Tasks;
using BarcodeLib;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace iAttendTFL_WebApp.Models
{
    public class account
    {
        public int id { get; set; }
        public string first_name { get; set; }
        public string last_name { get; set; }
        public string email { get; set; }
        public string salt { get; set; }
        
        [StringLength(200, ErrorMessage = "The password must be at least 8 characters long.", MinimumLength = 8)]
        [RegularExpression("^((?=.*?[A-Z])(?=.*?[a-z])(?=.*?[0-9])|(?=.*?[A-Z])(?=.*?[a-z])(?=.*?[^a-zA-Z0-9])|(?=.*?[A-Z])(?=.*?[0-9])(?=.*?[^a-zA-Z0-9])|(?=.*?[a-z])(?=.*?[0-9])(?=.*?[^a-zA-Z0-9])).{8,}$", ErrorMessage = "Passwords must be at least 8 characters and contain at least 3 of 4 of the following: upper case (A-Z), lower case (a-z), number (0-9) and special character (e.g. !@#$%^&*)")]
        public string password_hash { get; set; }

        [NotMapped] public string password_confirm { get; set; }

        public char account_type { get; set; }
        public bool email_verified { get; set; } = false;
        [NotMapped] public string gradMonth { get; set; }
        [NotMapped] public string gradYear { get; set; }
        public DateTime expected_graduation_date { get; set; }
        public int track_id { get; set; }
        public byte[] barcode { get; set; }
        public virtual ICollection<account_attendance> account_attendances { get; set; }
        public virtual ICollection<token> tokens { get; set; }

        // BARCODE GENERATION METHODS
        public Image StringToBarcodeImage(String input)
        {
            var barcodeMaker = new BarcodeLib.Barcode();
            Image myBarcode = barcodeMaker.Encode(BarcodeLib.TYPE.CODE39, input);
            return myBarcode;
        }

        public byte[] ImageToByteArray(Image img)
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



    }
}