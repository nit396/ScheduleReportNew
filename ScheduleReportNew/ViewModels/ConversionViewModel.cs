using ScheduleReport.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ScheduleReport.ViewModels
{
   public class ConversionViewModel
   {
      public string Comment { get; set; }
      public HttpPostedFileBase FileUpload { get; set; }
      public List<Conversion> ListConversion { get; set; }
   }
}