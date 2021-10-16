using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace YoklamaSeysi
{
    public partial class MainPage : ContentPage
    {
        public MainPage()
        {
            InitializeComponent();
        }

        public void LoadAddClassPage(object obj, EventArgs arg)
        {
            App.Current.MainPage = new AddClassPage();
        }

        //public void OnStepperValueChanged(object sender, ValueChangedEventArgs e)
        //{
        //    AttendanceViewModel avm = BindingContext as AttendanceViewModel;
        //    Stepper stepper = sender as Stepper;

        //    int newValue = Convert.ToInt32(e.NewValue);
        //    int oldValue = Convert.ToInt32(e.OldValue);
        //    int dif = newValue - oldValue;
        //    string className = stepper.ClassId.ToString();

        //    avm.UpdateAbsency(className, dif);
        //}
    }
}
