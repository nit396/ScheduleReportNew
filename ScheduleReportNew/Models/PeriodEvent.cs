using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ScheduleReport.Models
{
   public class PeriodEvent
   {
      public int Id { get; set; }
      public string Title { get; set; }
      public string Content { get; set; }
      public DateTime DateStart { get; set; }
      public DateTime DateEnd { get; set; }
      public DateTime TimeStart { get; set; }
      public DateTime TimeEnd { get; set; }
      public string CreatedUserId { get; set; }
      public string DaySelect { get; set; }
      public int GroupId { get; set; }
   }
}