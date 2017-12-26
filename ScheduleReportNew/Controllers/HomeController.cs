using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Microsoft.AspNet.Identity;
using ScheduleReportNew.Extensions;
using System.Globalization;
using ScheduleReport.Models;
using ScheduleReport.Utils;
using ScheduleReport.ViewModels;
using System.IO;

namespace ScheduleReport.Controllers
{

   public class HomeController : Controller
   {
      private static AppDbContext _dbContext;
      public HomeController()
      {
         _dbContext = new AppDbContext();
      }

      public ActionResult Index(DateTime? dateSelect = null)
      {
         if(!Request.IsAuthenticated)
         {
            return RedirectToAction("Login", "Account");
         }

         string userId = User.Identity.GetUserId();
         var user = _dbContext.Users.FirstOrDefault(n => n.Id.Equals(userId));

         if(dateSelect == null)
         {
            dateSelect = DateTime.Now;
         }

         ViewBag.Monday = dateSelect.Value.StartOfWeek();
         ViewBag.Sunday = dateSelect.Value.EndOfWeek();
         ViewBag.DateSelect = dateSelect.Value;

         ViewBag.NowUserId = userId;

         ViewBag.UIType = user.Type;

         List<TimeEvent> listEvent = SqlUtils.GetListTimeEvent(userId, dateSelect.Value);

         return View(listEvent);
      }

      [HttpPost]
      public ActionResult Index(string selectedDate)
      {
         if (!Request.IsAuthenticated)
         {
            return RedirectToAction("Login", "Account");
         }

         DateTime date;
         try
         {
            date = DateTime.ParseExact(selectedDate, "dd/MM/yyyy", CultureInfo.InvariantCulture);
         }
         catch(Exception)
         {
            date = DateTime.Now;
         }

         return RedirectToAction("Index", new { dateSelect = date });
      }

      public ActionResult Detail(int TEventId)
      {
         if (!Request.IsAuthenticated)
         {
            return RedirectToAction("Login", "Account");
         }

         var TEvent = _dbContext.TimeEvents.FirstOrDefault(n => n.Id == TEventId);

         var group = _dbContext.Groups.FirstOrDefault(n => n.Id == TEvent.GroupId);

         var listTEventDetail = _dbContext.TimeEventDetails.Where(n => n.TE_Id == TEventId).ToList<TimeEventDetail>();

         ViewBag.DateReport = TEvent.Date;
         ViewBag.GroupName = group.Name;

         return PartialView(listTEventDetail);
      }

      public ActionResult Conversion(int TEDetail_Id)
      {
         if (!Request.IsAuthenticated)
         {
            return RedirectToAction("Login", "Account");
         }

         var userId = User.Identity.GetUserId();
         var user = _dbContext.Users.FirstOrDefault(n => n.Id.Equals(userId));

         var TEventDetail = _dbContext.TimeEventDetails.FirstOrDefault(n => n.Id == TEDetail_Id);

         List<Conversion> listConversion = _dbContext.Conversions.Where(n => n.TimeEventDetailId == TEDetail_Id).OrderByDescending(n=>n.Date).ToList<Conversion>();

         ConversionViewModel viewModel = new ConversionViewModel();
         viewModel.ListConversion = listConversion;

         ViewBag.NowTimeEventDetailId = TEDetail_Id;
         ViewBag.UIType = user.Type;
         ViewBag.UserReportName = SqlUtils.GetUserFullName(TEventDetail.UserId);

         return PartialView(viewModel);
      }

      [HttpPost]
      public ActionResult Conversion(int TEDetail_Id, ConversionViewModel viewModel)
      {
         if (!Request.IsAuthenticated)
         {
            return RedirectToAction("Login", "Account");
         }

         string userId = User.Identity.GetUserId();
         var user = _dbContext.Users.FirstOrDefault(n => n.Id.Equals(userId));

         Conversion conversion = new Conversion();
         conversion.Date = DateTime.Now;
         conversion.TimeEventDetailId = TEDetail_Id;
         conversion.UserId = userId;

         if (!string.IsNullOrEmpty(viewModel.Comment) && viewModel.FileUpload == null)
         {
            conversion.Comment = viewModel.Comment;
            conversion.Name = "Comment";
            conversion.Type = 1;

            _dbContext.Conversions.Add(conversion);
         }
         else if (string.IsNullOrEmpty(viewModel.Comment) && viewModel.FileUpload != null)
         {
            string path = "~/FileUpload/" + userId + "/" + viewModel.FileUpload.FileName;
            string realPath = Server.MapPath(path);

            if (!Directory.Exists(Path.GetDirectoryName(realPath)))
               Directory.CreateDirectory(Path.GetDirectoryName(realPath));

            conversion.Name = viewModel.FileUpload.FileName;
            conversion.FilePath = path;
            conversion.Type = 2;

            viewModel.FileUpload.SaveAs(realPath);

            _dbContext.Conversions.Add(conversion);
         }
         else if (!string.IsNullOrEmpty(viewModel.Comment) && viewModel.FileUpload != null)
         {
            string path = "~/FileUpload/" + userId + "/" + viewModel.FileUpload.FileName;
            string realPath = Server.MapPath(path);

            if (!Directory.Exists(Path.GetDirectoryName(realPath)))
               Directory.CreateDirectory(Path.GetDirectoryName(realPath));

            conversion.Comment = viewModel.Comment;
            conversion.Name = viewModel.FileUpload.FileName;
            conversion.FilePath = path;
            conversion.Type = 3;

            viewModel.FileUpload.SaveAs(realPath);

            _dbContext.Conversions.Add(conversion);
         }

         if (user.Type == 2)
         {
            TimeEventDetail TEDetail = _dbContext.TimeEventDetails.FirstOrDefault(n => n.Id == TEDetail_Id);
            TEDetail.ReportStatus = true;
            _dbContext.Entry(TEDetail).State = System.Data.Entity.EntityState.Modified;
         }

         _dbContext.SaveChanges();

         TempData["EditTEDetailId"] = TEDetail_Id;
         return RedirectToAction("Index");
      }

      public FileResult Download(string downloadPath)
      {
         return File(System.IO.File.ReadAllBytes(Server.MapPath(downloadPath)), System.Net.Mime.MediaTypeNames.Application.Octet, Path.GetFileName(downloadPath));
      }
   }
}