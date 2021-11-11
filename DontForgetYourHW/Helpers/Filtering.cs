using DontForgetYourHW.Database;
using System;
using System.Collections.Generic;
using System.Text;
using Discord;

namespace DontForgetYourHW.Helpers
{
    public static class Filtering
    {
        #region Properties
        public static List<int> _intList { get; set; }
        #endregion

        #region Methods
        public static string DaysToString(string _days)
        {
            var format = "";
            var intList = new List<int>();

            if (_days.ToLower().Contains("m"))
            {
                format = "Mon";
                intList.Add(0);
            }

            if (_days.ToLower().Contains("t"))
            {
                if (_days.Length == 1) { format += (format == "") ? "Tues" : "/Tues"; }

                else
                {
                    var index = _days.ToLower().IndexOf("t");
                    var length = 2;

                    var substring = _days.ToLower().Substring(index, length);
                    if (substring != "th") { format += (format == "") ? "Tues" : "/Tues"; }
                }
                intList.Add(1);
            }

            format = CheckIfHas(format, _days, "w", "Wed", intList);
            format = CheckIfHas(format, _days, "h", "Thur", intList);
            format = CheckIfHas(format, _days, "f", "Fri", intList);
            _intList = intList;
            return format;
        }

        public static string GetCourseTitleString(Course _course)
        {
            var day = "";
            switch (_course.DayIndex)
            {
                case 0: day = "Mon"; break;
                case 1: day = "Tues"; break;
                case 2: day = "Wed"; break;
                case 3: day = "Thur"; break;
                case 4: day = "Fri"; break;
            }

            var spaceBeforeDay = (day == "Mon" || day == "Wed" || day == "Fri") ? " " : "";

            var nameSpace = GetExactSpace(_course.CourseName, 19);

            var abb = $"[{ _course.Abbreviation.ToUpper()}]";
            var abbSpace = GetExactSpace(abb, 6);

            var start = (DateTime)_course.StartTime;
            var end = (DateTime)_course.EndTime;
            var startHours = start.Hour;
            var endHours = end.Hour;

            var startSpace = (startHours >= 10 && startHours <= 12) ? "" : " ";

            var endSpace = (endHours >= 10 && endHours <= 12) ? "" : " ";

            return $"```yaml\n{spaceBeforeDay}{day} | {_course.CourseName}{nameSpace} | {abb}{abbSpace} | " +
                $"{startSpace}{start.ToShortTimeString()} - {endSpace}{end.ToShortTimeString()}```";
        }

        public static DateTime GetTime(bool _isStart, string _time)
        {
            if (_isStart)
            {
                var index = _time.IndexOf("-");
                var substring = _time.Substring(0, index);
                return Convert.ToDateTime(substring);
            }
            else
            {
                var index = _time.IndexOf("-") + 1;
                var length = _time.Length - index;
                var substring = _time.Substring(index, length);
                return Convert.ToDateTime(substring);
            }
        }

        public static string CheckIfHas(string _format,
            string _days, string _letter, string _word, List<int> _list)
        {
            if (_days.ToLower().Contains(_letter))
            {
                _format = (_format == "") ? _format = _word : _format += $"/{_word}";

                switch (_letter)
                {
                    case "w": _list.Add(2); break;
                    case "h": _list.Add(3); break;
                    case "f": _list.Add(4); break;
                }
            }
            return _format;
        }

        public static string GetHomeworkString(Homework _homework)
        {
            var left = (DateTime)_homework.Due - DateTime.Now;
            var day = left.Days;
            var hour = left.Hours;
            var min = left.Minutes;

            var fullString = "";
            if (day > 0)
                fullString += $"({day})days";

            if (hour > 0)
                fullString += (day > 0) ? $" ({hour})hr" : $"({hour})hr";

            if (min > 0)
                fullString += (day > 0) ? $" ({min})min" : $"({min})min";

            var abbString = $"[{_homework.Abbreviation.ToUpper()}]";
            var abbSpace = GetExactSpace(abbString, 6);

            var nameString = $"{_homework.Name}";
            var nameSpace = GetExactSpace(nameString, 23);

            var actualDate = (DateTime)_homework.Due;
            var dateString = actualDate.ToString("MMM dd h:mmtt");

            var bottomSpace = GetExactSpace(fullString, 30);

            var wholeBottomString = fullString + bottomSpace;

            var boolString = GetBoolString(_homework.IsRemindEnabled);

            return $"```yaml\n{abbString}{abbSpace} {nameString}{nameSpace}Reminder: {boolString}"+
                    $"\n{wholeBottomString}{dateString}\n```";
        }

        public static string GetHomeworkRecordString(Homework _homework)
        {
            var nameSpace = GetExactSpace(_homework.Name, 15);

            var courseSpace = GetExactSpace(_homework.CourseName, 19);

            var actualDate = (DateTime)_homework.Due;
            var dateString = actualDate.ToString("MMM dd, yyyy h:mmtt");

            var isCompletedString = (_homework.IsDone) ? "Completed" : "Uncompleted";

            var isCompletedSpace = GetExactSpace(isCompletedString, 11);

            return $"{_homework.Name}{nameSpace} | {_homework.CourseName}{courseSpace} |" +
                $" {isCompletedString}{isCompletedSpace} | {dateString}\n";
        }

        public static string GetExactSpace(string str, int maxLength)
        {
            var spaceLength = maxLength - str.Length;
            var howLongBlankString = "";
            for (int i = 0; i < spaceLength; i++)
                howLongBlankString += " ";

            return howLongBlankString;
        }

        public static string GetBoolString(bool _reminder)
        {
            var boolString = "";

            if (_reminder)
                boolString = "On";

            else
                boolString = "Off";

            return boolString;
        }

        /// <summary>
        ///     Set random number in between 0 to 256
        ///     for setting random color.
        /// </summary>
        /// <returns></returns>
        public static int X()
        {
            var random = new Random();
            var num = random.Next(0, 256);
            return num;
        }
        #endregion
    }
}
