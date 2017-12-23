using Microsoft.AspNet.Identity.EntityFramework;
using MySql.Data.Entity;
using ScheduleReport.Models;
using ScheduleReport.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace ScheduleReport.Controllers
{
   public class AppDbContext : IdentityDbContext<User>
   {
      public DbSet<PeriodEvent> PeriodEvents { get; set; }
      public DbSet<TimeEvent> TimeEvents { get; set; }
      public DbSet<TimeEventDetail> TimeEventDetails { get; set; }
      public DbSet<Group> Groups { get; set; }
      public DbSet<GroupDetail> GroupDetails { get; set; }
      public DbSet<Conversion> Conversions { get; set; }

      public AppDbContext() : base("DbContextConnectionString")
      {
      }

      public static AppDbContext Create()
      {
         return new AppDbContext();
      }
   }
}