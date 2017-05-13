using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Piller.Data;
using Java.Util;

namespace Piller.Droid.Extensions
{
    public static class DaysOfWeekExtensions
    {
       
        public static List<int> GetListDaysInInts(this DaysOfWeek days)
        {
            var listOfDays = new List<int>();

            if (days.HasFlag(DaysOfWeek.Monday))
                listOfDays.Add(Calendar.Monday);
            if (days.HasFlag(DaysOfWeek.Tuesday))
                listOfDays.Add(Calendar.Tuesday);
            if (days.HasFlag(DaysOfWeek.Wednesday))
                listOfDays.Add(Calendar.Wednesday);
            if (days.HasFlag(DaysOfWeek.Thursday))
                listOfDays.Add(Calendar.Thursday);
            if (days.HasFlag(DaysOfWeek.Friday))
                listOfDays.Add(Calendar.Friday);
            if (days.HasFlag(DaysOfWeek.Saturday))
                listOfDays.Add(Calendar.Saturday);
            if (days.HasFlag(DaysOfWeek.Sunday))
                listOfDays.Add(Calendar.Sunday);

            return listOfDays;
        }
        
    }
}