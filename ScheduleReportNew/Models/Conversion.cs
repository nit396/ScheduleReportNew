using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ScheduleReport.Models
{
   //Conversion Model For Report Status And Upload File
   public class Conversion
   {
      public int Id { get; set; }
      public DateTime Date { get; set; }
      public int Type { get; set; }
      public string FilePath { get; set; }
      public string Name { get; set; }
      public string Comment { get; set; }
      public string UserId { get; set; }
      public int TimeEventDetailId { get; set; }
   }
}