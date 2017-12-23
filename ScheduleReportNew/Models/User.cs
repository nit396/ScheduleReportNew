using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using System.Web;

namespace ScheduleReport.Models
{
   public class User : IdentityUser
   {
      public async Task<ClaimsIdentity> GenerateUserIdentityAsync(UserManager<User> manager)
      {
         // Note the authenticationType must match the one defined in CookieAuthenticationOptions.AuthenticationType
         var userIdentity = await manager.CreateIdentityAsync(this, DefaultAuthenticationTypes.ApplicationCookie);
         // Add custom user claims here
         return userIdentity;
      }

      public string FirstName { get; set; }
      public string LastName { get; set; }
      public int Type { get; set; }
      public string CreatedUserId { get; set; }
   }
}