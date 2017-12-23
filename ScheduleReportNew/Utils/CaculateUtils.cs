using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ScheduleReport.Utils
{
   public class CaculateUtils
   {
      public static int DayNumber(DayOfWeek DoW)
      {
         switch(DoW)
         {
            case DayOfWeek.Monday: return 1;
            case DayOfWeek.Tuesday: return 2;
            case DayOfWeek.Wednesday: return 3;
            case DayOfWeek.Thursday: return 4;
            case DayOfWeek.Friday: return 5;
            case DayOfWeek.Saturday: return 6;
            case DayOfWeek.Sunday: return 7;
         }
         return 0;
      }

      public static double HeightTimeEvent(DateTime timeStart, DateTime timeEnd)
      {
         return (timeEnd.Hour * 60 + timeEnd.Minute) - (timeStart.Hour * 60 + timeStart.Minute);
      }

      public static double TopTimeEvent(DateTime time)
      {
         return time.Hour * 60 + time.Minute;
      }
   }
}