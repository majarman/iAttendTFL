using iAttendTFL_MobileApp.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace iAttendTFL_MobileApp
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class MainPage : ContentPage
    {
        public MainPage()
        {
            InitializeComponent();
        }
        void OnSubmitButtonClicked(object sender, EventArgs eventArgs)
        {

            Submission sendme = new Submission(int.Parse(idEditor.Text), nameEditor.Text);

        }
    }
}