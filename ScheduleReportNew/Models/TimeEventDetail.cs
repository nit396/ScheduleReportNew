using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ScheduleReport.Models
{
   public class TimeEventDetail
   {
      public int Id { get; set; }
      public int TE_Id { get; set; }
      public string UserId { get; set; }
      public bool ReportStatus { get; set; }
   }
}