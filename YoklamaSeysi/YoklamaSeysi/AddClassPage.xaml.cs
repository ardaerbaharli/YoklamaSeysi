using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace YoklamaSeysi
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class AddClassPage : ContentPage
    {
        public AddClassPage()
        {
            InitializeComponent();
        }
        public void Add(object obj, EventArgs args)
        {
            // GoBack(obj,args);
        }

        public void GoBack(object obj, EventArgs args)
        {
            App.Current.MainPage = new MainPage();
        }
    }
}