﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Timetable.Domain.ExtensionMethods
{
    public static class WeekExtension
    {
        public static void OrderByDay(this Week value)
        {
            var day1 = value.OneDayTimetables?.FirstOrDefault(e => e.Day == "Понедельник");
            var day2 = value.OneDayTimetables?.FirstOrDefault(e => e.Day == "Вторник"); 
            var day3 = value.OneDayTimetables?.FirstOrDefault(e => e.Day == "Среда");
            var day4 = value.OneDayTimetables?.FirstOrDefault(e => e.Day == "Четверг"); 
            var day5 = value.OneDayTimetables?.FirstOrDefault(e => e.Day == "Пятница");
            var day6 = value.OneDayTimetables?.FirstOrDefault(e => e.Day == "Суббота");

            List<OneDayTimetable> days = new List<OneDayTimetable>
            {
                null,
                day1,
                day2,
                day3,
                day4,
                day5,
                day6,
                null
            };

            days.RemoveAll(p => p == null);

            value.OneDayTimetables = days;
        }
    }
}
