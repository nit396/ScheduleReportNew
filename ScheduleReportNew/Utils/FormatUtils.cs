using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ScheduleReport.Utils
{
   public class FormatUtils
   {
      public static string StatusString(bool isreported)
      {
         return (isreported == true) ? "Đã báo cáo ✔" : "Chưa báo cáo ✘";
      }
      public static string GetDayOfWeek(DayOfWeek DoW)
      {
         switch(DoW)
         {
            case DayOfWeek.Monday: return "Hai";
            case DayOfWeek.Tuesday: return "Ba";
            case DayOfWeek.Wednesday: return "Tư";
            case DayOfWeek.Thursday: return "Năm";
            case DayOfWeek.Friday: return "Sáu";
            case DayOfWeek.Saturday: return "Bảy";
            case DayOfWeek.Sunday: return "Chủ Nhật";
         }
         return "Unknown";
      }

      public static string GetDayFromId(int id)
      {
         switch(id)
         {
            case 0: return "Hai";
            case 1: return "Ba";
            case 2: return "Tư";
            case 3: return "Năm";
            case 4: return "Sáu";
            case 5: return "Bảy";
            case 6: return "Chủ Nhật";
         }
         return "";
      }

      public static string GetTimeShow(int time)
      {
         return TimeSpan.FromHours(time).ToString("hh':'mm");
      }

      public static string GetDaySelect(bool[] listCheck)
      {
         string result = "";
         foreach(bool check in listCheck)
         {
            result += (check == true) ? "1" : "0";
         }
         return result;
      }

      public static DayOfWeek DoWFromNumber(int number)
      {
         switch (number)
         {
            case 1: return DayOfWeek.Monday;
            case 2: return DayOfWeek.Tuesday;
            case 3: return DayOfWeek.Wednesday;
            case 4: return DayOfWeek.Thursday;
            case 5: return DayOfWeek.Friday;
            case 6: return DayOfWeek.Saturday;
            case 7: return DayOfWeek.Sunday;
         }
         return DayOfWeek.Monday;
      }

      public static List<DayOfWeek> ConvertDaySelectToDoW(string dayselect)
      {
         if (dayselect.Length != 7) return null;

         List<DayOfWeek> listDay = new List<DayOfWeek>();
         for (int i = 0; i < dayselect.Length; i++)
         {
            if (dayselect[i] == '1')
            {
               DayOfWeek day = DoWFromNumber(i + 1);
               listDay.Add(day);
            }
         }
         return listDay;
      }

      public static bool[] ConvertDaySelectToBool(string dayselect)
      {
         if (dayselect.Length != 7) return null;

         List<bool> listDay = new List<bool>();
         for (int i = 0; i < dayselect.Length; i++)
         {
            listDay.Add(dayselect[i] == '1' ? true : false);
         }
         return listDay.ToArray();
      }
   }
}