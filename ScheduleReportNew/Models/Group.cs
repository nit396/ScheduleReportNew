using ScheduleReport.Controllers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;


namespace ScheduleReport.Models
{
   public class Group
   {
      public int Id { get; set; }
      public string Name { get; set; }
      public DateTime Date { get; set; }
      public string CreatedUserId { get; set; }

      public int CountUser()
      {
         AppDbContext _dbContext = new AppDbContext();
         int count = _dbContext.GroupDetails.Count(n => n.GroupId.Equals(Id));
         return count;
      }
   }
}