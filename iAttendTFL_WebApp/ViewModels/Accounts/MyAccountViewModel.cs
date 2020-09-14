namespace iAttendTFL_WebApp.ViewModels.Accounts
{
    public class MyAccountViewModel
    {
        public string FullName { get; set; }
        public string Email { get; set; }
        public string AccountTypeString { get; set; }
        public string Track { get; set; }
        public string ExpectedGraduationDate { get; set; }
        public char AccountType { get; set; }
        public byte[] Barcode { get; set; }
    }
}
