using System;
using System.Globalization;

namespace Tahlile_Parseh.Controllers
{
    public class CalendarManager
    {
        public static DateTime PersianToJulian(string publishdatepersian)
        {
            PersianCalendar p = new PersianCalendar();
            string[] parts = publishdatepersian.Split('/', '-');
            return p.ToDateTime(
                Convert.ToInt32(parts[0]),
                Convert.ToInt32(parts[1]),
                Convert.ToInt32(parts[2]), 0, 0, 0, 0);
        }
        public static string JulianToPersian(DateTime dt)
        {
            PersianCalendar p = new PersianCalendar();
            return $"{p.GetYear(dt)}/{p.GetMonth(dt)}/{p.GetDayOfMonth(dt)}";
        }
    }
}