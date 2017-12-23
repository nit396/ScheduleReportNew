namespace ScheduleReport.Migrations
{
   using Microsoft.AspNet.Identity;
   using ScheduleReport.Controllers;
using System;
   using System.Data.Entity;
   using System.Data.Entity.Migrations;
   using System.Linq;

   internal sealed class Configuration : DbMigrationsConfiguration<ScheduleReport.Controllers.AppDbContext>
   {
      public Configuration()
      {
         AutomaticMigrationsEnabled = false;
      }

      protected override void Seed(ScheduleReport.Controllers.AppDbContext context)
      {
         var _dbContext = new AppDbContext();
         _dbContext.Users.AddOrUpdate(new Models.User { UserName = "ad", PasswordHash = new PasswordHasher().HashPassword("12"), FirstName = "Mai", LastName = "Sỹ Anh", SecurityStamp= Guid.NewGuid().ToString()});
         _dbContext.SaveChanges();
      }
   }
}
