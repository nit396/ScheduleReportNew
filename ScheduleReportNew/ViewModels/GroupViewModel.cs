using ScheduleReport.Controllers;
using ScheduleReport.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Microsoft.AspNet.Identity;

namespace ScheduleReport.ViewModels
{
   public class GroupViewModel
   {
      public int Id { get; set; }
      public string Name { get; set; }
      public DateTime Date { get; set; }
      public List<User> GetUsers()
      {
         AppDbContext _dbContext = new AppDbContext();
         List<User> result = new List<User>();
         var listGroupDetail = _dbContext.GroupDetails.Where(n => n.GroupId.Equals(Id)).ToList<GroupDetail>();
         foreach (var groupDetail in listGroupDetail)
         {
            var user = _dbContext.Users.FirstOrDefault(n => n.Id.Equals(groupDetail.MemberUserId));
            if(user !=null)
               result.Add(user);
         }
         return result;
      }
   }
}