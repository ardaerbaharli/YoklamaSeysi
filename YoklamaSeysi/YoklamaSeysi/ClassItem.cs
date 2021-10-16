using SQLite;
using System;

namespace YoklamaSeysi
{
    [Table("Classes")]
    public class ClassItem
    {
        [PrimaryKey, AutoIncrement]
        public int ID { get; set; }
        public string ClassName { get; set; }
        public int AbsentCount { get; set; }
        public int ClassCountPerWeek { get; set; }
        public int RemainingAttendance { get; set; }
        public int FailurePercent { get; set; }
        public int TotalWeekCount { get; set; }

        public void IncrementAbsent()
        {
            AbsentCount++;
            RemainingAttendance--;
        }

        public void DecrementAbsent()
        {
            AbsentCount--;
            RemainingAttendance++;
        }

        public void AddAbsency(int days)
        {
            if (AbsentCount + days < 0)
                return;
            if (RemainingAttendance - days < 0)
                return;

            AbsentCount += days;
            RemainingAttendance -= days;
        }

        public void CalculateRemainingAttendance()
        {
            RemainingAttendance = ClassCountPerWeek * TotalWeekCount / 100 * (100 - FailurePercent);
        }
    }
}
