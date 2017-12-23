using ScheduleReport.Controllers;
using ScheduleReport.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ScheduleReport.ViewModels
{
   public class ScheduleViewModel
   {
      public int Id { get; set; }
      public string Title { get; set; }
      public string Content { get; set; }
      public string DateStart { get; set; }
      public string DateEnd { get; set; }
      public string TimeStart { get; set; }
      public string TimeEnd { get; set; }
      public bool[] DaySelect { get; set; }
      public int GroupId { get; set; }
   }
}