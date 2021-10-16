using SQLite;
using System;

namespace YoklamaSeysi
{
    [Table("Classes")]
    public class ClassItem : ObservableProperty
    {
        private int id;
        private string className;
        private int absentCount;
        private int classCountPerWeek;
        private int remainingAttendance;
        private int failurePercent;
        private int totalWeekCount;

        [PrimaryKey, AutoIncrement]
        public int ID
        {
            get => id;
            set
            {
                id = value;
                OnPropertyChanged("ID");
            }
        }
        public string ClassName
        {
            get => className;
            set
            {
                className = value;
                OnPropertyChanged("ClassName");
            }
        }
        public int AbsentCount
        {
            get => absentCount;
            set
            {
                absentCount = value;
                OnPropertyChanged("AbsentCount");
            }
        }
        public int ClassCountPerWeek
        {
            get => classCountPerWeek;
            set
            {
                classCountPerWeek = value;
                OnPropertyChanged("ClassCountPerWeek");
            }
        }
        public int RemainingAttendance
        {
            get => remainingAttendance;
            set
            {
                remainingAttendance = value;
                OnPropertyChanged("RemainingAttendance");
            }
        }
        public int FailurePercent
        {
            get => failurePercent;
            set
            {
                failurePercent = value;
                OnPropertyChanged("FailurePercent");
            }
        }
        public int TotalWeekCount
        {
            get => totalWeekCount;
            set
            {
                totalWeekCount = value;
                OnPropertyChanged("TotalWeekCount");
            }
        }

        public void IncreaseAbcency()
        {
            AbsentCount++;
            RemainingAttendance--;
        }
        public void DecreaseAbcency()
        {
            AbsentCount--;
            RemainingAttendance++;
        }
        public void CalculateRemainingAttendance()
        {
            RemainingAttendance = (int)Math.Floor(ClassCountPerWeek * TotalWeekCount / 100d * (100 - FailurePercent));
        }
    }
}
