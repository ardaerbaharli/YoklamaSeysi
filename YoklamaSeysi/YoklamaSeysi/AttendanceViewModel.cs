using System;
using System.Collections.ObjectModel;
using System.Windows.Input;
using Xamarin.Forms;
using SQLite;
using System.Text.RegularExpressions;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace YoklamaSeysi
{
    class AttendanceViewModel
    {
        public ObservableCollection<ClassItem> Classes { get; set; }
        public AttendanceViewModel()
        {
            Classes = new ObservableCollection<ClassItem>();
            TotalWeekCountInput = App.TotalWeekCount.ToString();

            Refresh();
        }

        public string ClassNameInput { get; set; }
        public string ClassCountPerWeekInput { get; set; }
        public string FailurePercentInput { get; set; }
        public string TotalWeekCountInput { get; set; }

        public ICommand AddClassCommand => new Command(Add);
        public void Add()
        {
            if (string.IsNullOrEmpty(ClassNameInput))
                return;
            if (string.IsNullOrEmpty(ClassCountPerWeekInput))
                return;
            if (string.IsNullOrEmpty(FailurePercentInput))
                return;
            if (string.IsNullOrEmpty(TotalWeekCountInput))
                return;

            int classCountPerWeek = int.Parse(ClassCountPerWeekInput);
            int failurePercent = int.Parse(FailurePercentInput);
            int totalWeekCount = int.Parse(TotalWeekCountInput);

            if (!Regex.IsMatch(ClassNameInput, "^[a-zA-Z0-9]*$"))
                return;

            if (classCountPerWeek < 0 || classCountPerWeek > 99)
                return;

            if (failurePercent < 0 || failurePercent > 100)
                return; 
            
            if (totalWeekCount < 0 || totalWeekCount > 100)
                return;            

            ClassItem classItem = new ClassItem()
            {
                ClassName = ClassNameInput,
                ClassCountPerWeek = classCountPerWeek,
                FailurePercent = failurePercent,
                TotalWeekCount = totalWeekCount
            };

            classItem.CalculateRemainingAttendance();

            using (SQLiteConnection conn = new SQLiteConnection(App.DatabasePath))
            {
                conn.CreateTable<ClassItem>();
                conn.Insert(classItem);
                conn.Close();
            }

            Classes.Add(classItem);
            App.Current.MainPage = new MainPage();

        }
        public ICommand RemoveClassCommand => new Command(Remove);
        public void Remove(object o)
        {
            ClassItem classItem = o as ClassItem;

            using (SQLiteConnection conn = new SQLiteConnection(App.DatabasePath))
            {
                conn.CreateTable<ClassItem>();
                conn.Delete(classItem);
                Classes.Remove(classItem);
                conn.Close();
            }
        }
        public void UpdateAbsency(string className, int days)
        {
            var item = Classes.Where(x => x.ClassName == className).FirstOrDefault();
            item.AddAbsency(days);
            var index = Classes.IndexOf(item);
            Classes[index] = item;

            Update();

        }
        public void Refresh()
        {
            using (SQLiteConnection conn = new SQLiteConnection(App.DatabasePath))
            {
                conn.CreateTable<ClassItem>();
                Classes = new ObservableCollection<ClassItem>(conn.Table<ClassItem>().ToList());
                conn.Close();
            }
        }
        public void Update()
        {
            using (SQLiteConnection conn = new SQLiteConnection(App.DatabasePath))
            {
                conn.UpdateAll(Classes);
                conn.Close();
            }
        }

       

    }
}
