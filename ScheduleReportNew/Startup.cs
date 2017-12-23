using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(ScheduleReport.Startup))]
namespace ScheduleReport
{
   public partial class Startup
   {
      public void Configuration(IAppBuilder app)
      {
         ConfigureAuth(app);
      }
   }
}
