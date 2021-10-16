using System;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace YoklamaSeysi
{
    public partial class App : Application
    {
        public static string DatabasePath = string.Empty;
        public static int TotalWeekCount = 0;
        public App(string databasePath)
        {
            InitializeComponent();

            DatabasePath = databasePath;
            TotalWeekCount = 14;
            App.Current.MainPage = new MainPage();
        }

        protected override void OnStart()
        {
        }

        protected override void OnSleep()
        {
        }

        protected override void OnResume()
        {
        }
    }
}
