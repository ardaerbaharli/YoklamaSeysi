using System.Collections.ObjectModel;
using System.Windows.Input;
using Xamarin.Forms;
using SQLite;
using System.Text.RegularExpressions;
using System.Linq;

namespace YoklamaSeysi
{
    class AttendanceViewModel : ObservableProperty
    {
        ObservableCollection<ClassItem> classes;
        public ObservableCollection<ClassItem> Classes
        {
            get { return classes; }
            set
            {
                classes = value;
                OnPropertyChanged("ClassItem");
            }
        }

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

        public ICommand AddClassCommand => new Command(AddClass);
        public void AddClass()
        {
            if (ValidateInputs())
            {
                var classItem = CreateClass();
                Add(classItem);
                App.Current.MainPage = new MainPage();      // go back to main page
            }
        }
        private ClassItem CreateClass()
        {
            int classCountPerWeek = int.Parse(ClassCountPerWeekInput);
            int failurePercent = int.Parse(FailurePercentInput);
            int totalWeekCount = int.Parse(TotalWeekCountInput);
            ClassItem classItem = new ClassItem()
            {
                ClassName = ClassNameInput,
                ClassCountPerWeek = classCountPerWeek,
                FailurePercent = failurePercent,
                TotalWeekCount = totalWeekCount
            };

            classItem.CalculateRemainingAttendance();
            return classItem;
        }
        private bool ValidateInputs()
        {
            if (string.IsNullOrEmpty(ClassNameInput)) return false;
            if (string.IsNullOrEmpty(ClassCountPerWeekInput)) return false;
            if (string.IsNullOrEmpty(FailurePercentInput)) return false;
            if (string.IsNullOrEmpty(TotalWeekCountInput)) return false;

            int classCountPerWeek = int.Parse(ClassCountPerWeekInput);
            int failurePercent = int.Parse(FailurePercentInput);
            int totalWeekCount = int.Parse(TotalWeekCountInput);

            if (!Regex.IsMatch(ClassNameInput, "^[a-zA-Z0-9]*$")) return false;
            if (classCountPerWeek < 1 || classCountPerWeek > 99) return false;
            if (failurePercent < 1 || failurePercent > 100) return false;
            if (totalWeekCount < 1 || totalWeekCount > 100) return false;

            return true;
        }
        public void Add(ClassItem classItem)
        {
            using (SQLiteConnection conn = new SQLiteConnection(App.DatabasePath))
            {
                conn.CreateTable<ClassItem>();
                conn.Insert(classItem);
                conn.Close();
            }

            Classes.Add(classItem);
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

        public ICommand DecreaseAbcencyCommand => new Command(DecreaseAbcency);

        private void DecreaseAbcency(object o)
        {
            ClassItem classItem = o as ClassItem;
            var item = Classes.Where(x => x == classItem).FirstOrDefault();

            if (classItem.AbsentCount - 1 < 0)
                return;

            item.DecreaseAbcency();
            Update();
        }

        public ICommand IncreaseAbcencyCommand => new Command(IncreaseAbcency);
        private void IncreaseAbcency(object o)
        {
            ClassItem classItem = o as ClassItem;
            var item = Classes.Where(x => x == classItem).FirstOrDefault();

            if (item.RemainingAttendance - 1 < 0)
                return;

            item.IncreaseAbcency();
            Update();
        }

        //public void UpdateAbsency(string className, int days)
        //{
        //    var item = Classes.Where(x => x.ClassName == className).FirstOrDefault();
        //    item.AddAbsency(days);
        //    var index = Classes.IndexOf(item);
        //    Classes[index] = item;

        //    Update();
        //}
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
