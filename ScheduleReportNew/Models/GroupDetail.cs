using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ScheduleReport.Models
{
   public class GroupDetail
   {
      public int Id { get; set; }
      public string MemberUserId { get; set; }
      public int GroupId { get; set; }
   }
}