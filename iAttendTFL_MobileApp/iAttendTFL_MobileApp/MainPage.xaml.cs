using iAttendTFL_MobileApp.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using ZXing;
using ZXing.Net.Mobile.Forms;

namespace iAttendTFL_MobileApp
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class MainPage : ContentPage
    {
        //private Button buttonScan;

        public MainPage()
        {
            //ZXingScannerPage scanPage = new ZXingScannerPage();
            //await Navigation.PushAsync(scanPage);

            //buttonScan.Click += (sender, e) =>
            //{

            //        #if __ANDROID__
	           //     // Initialize the scanner first so it can track the current context
	           //     MobileBarcodeScanner.Initialize (Application);
            //        #endif

            //    var scanner = new ZXing.Mobile.MobileBarcodeScanner();

            //    var result = await scanner.Scan();

            //    if (result != null)
            //        Console.WriteLine("Did you mean to scan " + result.Text + "?");
            //};

            InitializeComponent();
        }

        public void Handle_OnScanResult(Result result)
        {
            //TODO: I think here or the layer below is where the code to find the user and add them into the event will go.
            Device.BeginInvokeOnMainThread(async () =>
            {
                await DisplayAlert("account.first_name +sp+ account.last_name", result.Text, "OK");
            });
        }
        //void OnSubmitButtonClicked(object sender, EventArgs eventArgs)
        //{

        //    Submission sendme = new Submission(int.Parse(idEditor.Text), nameEditor.Text);

        //}


    }
}