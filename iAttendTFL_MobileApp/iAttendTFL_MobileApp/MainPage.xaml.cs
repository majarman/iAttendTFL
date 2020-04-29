using iAttendTFL_MobileApp.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
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

        public MainPage()
        {
            InitializeComponent();
        }

        public async void Handle_OnScanResult(Result result)
        {
            //TODO: I think here or the layer below is where the code to find the user and add them into the event will go.
            int scanResultInt = int.Parse(result.Text);
            //if (accountExists(scanResultInt))
            //{

            account_attendance workingCheckin = new account_attendance();
            //if (notYetScanned(scanResultInt))
            //{

            workingCheckin.account_id = scanResultInt; //workingAccount.id;
            workingCheckin.scan_event_id = 2;
            workingCheckin.is_valid = true;
            workingCheckin.attendance_time = DateTime.Now;
            await enterEvent(workingCheckin);
            System.Diagnostics.Debug.WriteLine("Account:");
            System.Diagnostics.Debug.WriteLine(getNameFromIdAsync(scanResultInt).Result);
            //Device.BeginInvokeOnMainThread(async () =>
            //    {
            //        await DisplayAlert("Scanned User: ", getNameFromIdAsync(scanResultInt).Result, "OK");
            //    });



            //} //notYetScanned
            //else { return RedirectToAction(nameof(Create)); }
            //return RedirectToAction(nameof(Index));

            //} // account does not already exist in database
            //else { return RedirectToAction(nameof(Create)); }

            //return RedirectToAction(nameof(Index));
            //}


        }


        //==========================
        //ustilized custom functions
        //==========================

        public static async Task<string> getNameFromIdAsync(int id)
        {
            var client = new HttpClient();
            string uri = "http://iattendapi.eastus.cloudapp.azure.com:3000/account?id=eq." + id.ToString() + "&select=first_name,last_name";

            String response = await client.GetStringAsync(uri);
            return response;
        }

        public static async Task<string> enterEvent(account_attendance workingModel)
        {
            var client = new HttpClient();
            Uri uri = new Uri("http://iattendapi.eastus.cloudapp.azure.com:3000");
            client.BaseAddress = uri;

            string jsonData = "{\"account_id\" : \"" + workingModel.account_id.ToString() + "\", \"scan_event_id\" : \"" + workingModel.scan_event_id.ToString() + "\", \"is_valid\" : \"" + workingModel.is_valid.ToString() + "\", \"attendance_time\" : \"" + workingModel.attendance_time.ToString() + "\"}";

            System.Diagnostics.Debug.WriteLine(jsonData);
            
            var content = new StringContent(jsonData, Encoding.UTF8, "application/json");
            
            HttpResponseMessage response = await client.PostAsync("/account_attendance", content);
            var result = await response.Content.ReadAsStringAsync();
            
            return result;
        }

        //private bool accountExists(int id)
        //{
        //    return _context.account.Any(e => e.id == id);
        //}

        //private bool notYetScanned(int resultInt, account_attendance )
        //{
        //    if (account_attendance.scan_event_id == 2) 
        //    { 
        //        return _context.account_attendance.Any(e => e.account_id == resultInt);
        //    }
        //    else { return false; }
        //}

    }
}