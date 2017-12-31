using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Microsoft.AspNet.Identity;
using ScheduleReport.Models;
using ScheduleReport.Controllers;
using ScheduleReport.ViewModels;
using ScheduleReport.Utils;
using System.Globalization;

namespace ScheduleReportNew.Controllers
{
   public class ScheduleController : Controller
   {
      private static AppDbContext _dbContext = new AppDbContext();
      // GET: Schedule
      public ActionResult Index()
      {
         if (!Request.IsAuthenticated)
         {
            return RedirectToAction("Login", "Account")  ;

         }

         string userId = User.Identity.GetUserId();
         List<PeriodEvent> listPEvent = _dbContext.PeriodEvents.Where(n => n.CreatedUserId.Equals(userId)).ToList<PeriodEvent>();

         return View(listPEvent);
      }

      public ActionResult Create()
      {
         if (!Request.IsAuthenticated)
         {
            return RedirectToAction("Login", "Account");

         }

         ViewBag.UserId = User.Identity.GetUserId();

         return PartialView();
      }

      [HttpPost]
      public ActionResult Create(ScheduleViewModel viewModel)
      {
         if (!Request.IsAuthenticated)
         {
            return RedirectToAction("Login", "Account");
         }

         string userId = User.Identity.GetUserId();

         PeriodEvent PEvent = new PeriodEvent();
         PEvent.Title = viewModel.Title;
         PEvent.Content = viewModel.Content;
         PEvent.CreatedUserId = userId;
         PEvent.DateStart = DateTime.ParseExact(viewModel.DateStart, "dd/MM/yyyy", CultureInfo.InvariantCulture);
         PEvent.DateEnd = DateTime.ParseExact(viewModel.DateEnd, "dd/MM/yyyy", CultureInfo.InvariantCulture);
         PEvent.TimeStart = DateTime.ParseExact(viewModel.TimeStart, "HH:mm", CultureInfo.InvariantCulture);
         PEvent.TimeEnd = DateTime.ParseExact(viewModel.TimeEnd, "HH:mm", CultureInfo.InvariantCulture);
         PEvent.DaySelect = FormatUtils.GetDaySelect(viewModel.DaySelect);
         PEvent.GroupId = viewModel.GroupId;
         _dbContext.PeriodEvents.Add(PEvent);
         _dbContext.SaveChanges();

         SqlUtils.AddTEventFromPEvent(PEvent);

         return RedirectToAction("Index", "Schedule");
      }

      public ActionResult Edit(int PEventId)
      {
         if (!Request.IsAuthenticated)
         {
            return RedirectToAction("Login", "Account");
         }

         ViewBag.UserId = User.Identity.GetUserId();

         var PEvent = _dbContext.PeriodEvents.FirstOrDefault(n => n.Id == PEventId);

         ScheduleViewModel viewModel = new ScheduleViewModel();
         viewModel.Title = PEvent.Title;
         viewModel.Id = PEvent.Id;
         viewModel.Content = PEvent.Content;
         viewModel.DateStart = PEvent.DateStart.ToString("dd/MM/yyyy");
         viewModel.DateEnd = PEvent.DateEnd.ToString("dd/MM/yyyy");
         viewModel.TimeStart = PEvent.TimeStart.ToString("HH:mm");
         viewModel.TimeEnd = PEvent.TimeEnd.ToString("HH:mm");
         viewModel.DaySelect = FormatUtils.ConvertDaySelectToBool(PEvent.DaySelect);
         viewModel.GroupId = PEvent.GroupId;

         return PartialView(viewModel);
      }

      [HttpPost]
      public ActionResult Edit(int PEventId, ScheduleViewModel viewModel)
      {
         if (!Request.IsAuthenticated)
         {
            return RedirectToAction("Login", "Account");
         }

         string userId = User.Identity.GetUserId();

         PeriodEvent PEvent = _dbContext.PeriodEvents.FirstOrDefault(n => n.Id.Equals(PEventId));
         PEvent.Title = viewModel.Title;
         PEvent.Content = viewModel.Content;
         PEvent.DateStart = DateTime.ParseExact(viewModel.DateStart, "dd/MM/yyyy", CultureInfo.InvariantCulture);
         PEvent.DateEnd = DateTime.ParseExact(viewModel.DateEnd, "dd/MM/yyyy", CultureInfo.InvariantCulture);
         PEvent.TimeStart = DateTime.ParseExact(viewModel.TimeStart, "HH:mm", CultureInfo.InvariantCulture);
         PEvent.TimeEnd = DateTime.ParseExact(viewModel.TimeEnd, "HH:mm", CultureInfo.InvariantCulture);
         PEvent.DaySelect = FormatUtils.GetDaySelect(viewModel.DaySelect);
         PEvent.GroupId = viewModel.GroupId;
         _dbContext.Entry(PEvent).State = System.Data.Entity.EntityState.Modified;
         _dbContext.SaveChanges();

         SqlUtils.UpdateTEventFromPEvent(PEvent);

         return RedirectToAction("Index", "Schedule");
      }
   }
}