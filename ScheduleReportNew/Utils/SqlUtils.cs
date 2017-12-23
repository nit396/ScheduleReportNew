using ScheduleReport.Controllers;
using ScheduleReport.Models;
using ScheduleReportNew.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ScheduleReport.Utils
{
   public class SqlUtils
   {
      private static AppDbContext _dbContext = new AppDbContext();

      public static int GetTimeEventDetailId(string userId, int TEventId)
      {
         return _dbContext.TimeEventDetails.FirstOrDefault(n => n.TE_Id == TEventId && n.UserId.Equals(userId)).Id;
      }

      public static List<TimeEvent> GetListTimeEvent(string userId, DateTime date)
      {
         var user = _dbContext.Users.FirstOrDefault(n => n.Id.Equals(userId));
         List<TimeEvent> result = new List<TimeEvent>();
         if(user.Type != 2)
         {
            List<TimeEvent> listTEvent = _dbContext.TimeEvents.Where(n=> n.CreatedUserId.Equals(userId)).ToList<TimeEvent>();
            foreach(TimeEvent TEvent in listTEvent)
            {
               if(TEvent.Date >= date.StartOfWeek().Date
                     && TEvent.Date <= date.EndOfWeek().Date)
               {
                  result.Add(TEvent);
               }
            }
         }
         else
         {
            var listUserGroup = _dbContext.GroupDetails.Where(n => n.MemberUserId.Equals(userId)).ToList<GroupDetail>();
            foreach(GroupDetail groupDetail in listUserGroup)
            {
               List<TimeEvent> listTEvent = _dbContext.TimeEvents.Where(n=> n.GroupId.Equals(groupDetail.GroupId)).ToList<TimeEvent>();

               foreach(TimeEvent TEvent in listTEvent)
               {
                  if (TEvent.Date >= date.StartOfWeek().Date
                     && TEvent.Date <= date.EndOfWeek().Date)
                  {
                     result.Add(TEvent);
                  }
               }
            }
         }
         return result;
      }

      public static string GetUserFullName(string userId)
      {
         var user = _dbContext.Users.FirstOrDefault(n => n.Id.Equals(userId));
         return user.FirstName + " " + user.LastName;
      }

      public static List<Group> GetListGroup(string userId)
      {
         return new AppDbContext().Groups.Where(n => n.CreatedUserId.Equals(userId)).ToList<Group>(); ;
      }

      public static int CountGroupUser(string userId)
      {
         return _dbContext.Groups.Count(n => n.CreatedUserId.Equals(userId));
      }

      public static void AddTEventFromPEvent(PeriodEvent PEvent)
      {
         List<DayOfWeek> list = FormatUtils.ConvertDaySelectToDoW(PEvent.DaySelect);
         for (DateTime date = PEvent.DateStart; date <= PEvent.DateEnd; date = date.AddDays(1))
         {
            if (list.Contains(date.DayOfWeek))
            {
               TimeEvent TEvent = new TimeEvent();
               TEvent.Title = PEvent.Title;
               TEvent.Content = PEvent.Content;
               TEvent.CreatedUserId = PEvent.CreatedUserId;
               TEvent.Date = date;
               TEvent.TimeStart = PEvent.TimeStart;
               TEvent.TimeEnd = PEvent.TimeEnd;
               TEvent.GroupId = PEvent.GroupId;
               TEvent.PE_Id = PEvent.Id;
               _dbContext.TimeEvents.Add(TEvent);
               _dbContext.SaveChanges();

               AddTEventDetailForGroup(TEvent.Id, PEvent.GroupId);
            }
         }
      }

      public static void AddTEventDetailForUser(int TEventId, string userId)
      {
         if (!_dbContext.TimeEventDetails.Any(n => n.UserId.Equals(userId) && n.TE_Id == TEventId))
         {
            TimeEventDetail TEventDetail = new TimeEventDetail();
            TEventDetail.TE_Id = TEventId;
            TEventDetail.UserId = userId;
            TEventDetail.ReportStatus = false;
            _dbContext.TimeEventDetails.Add(TEventDetail);
            _dbContext.SaveChanges();
         }
      }

      public static void AddTEventDetailForGroup(int TEventId, int groupId)
      {
         List<GroupDetail> listGroupDetail = _dbContext.GroupDetails.Where(n => n.GroupId == groupId).ToList<GroupDetail>();
         foreach(GroupDetail groupDetail in listGroupDetail)
         {
            AddTEventDetailForUser(TEventId, groupDetail.MemberUserId);
         }
      }

      public static void DeleteTimeEvent(TimeEvent TEvent)
      {
         List<TimeEventDetail> listTEventDetail = _dbContext.TimeEventDetails.Where(n => n.TE_Id == TEvent.Id).ToList<TimeEventDetail>();
         foreach(TimeEventDetail TEventDetail in listTEventDetail)
         {
            _dbContext.Entry(TEventDetail).State = System.Data.Entity.EntityState.Deleted;
         }
         _dbContext.SaveChanges();
      }

      public static void UpdateTEventFromPEvent(PeriodEvent PEvent)
      {

         #region Update Time Event By Period
         List<TimeEvent> listTEvent = _dbContext.TimeEvents.Where(n => n.PE_Id == PEvent.Id).ToList<TimeEvent>();
         List<DayOfWeek> listDoW = FormatUtils.ConvertDaySelectToDoW(PEvent.DaySelect);
         foreach (TimeEvent TEvent in listTEvent)
         {
            TEvent.TimeStart = PEvent.TimeStart;
            TEvent.TimeEnd = PEvent.TimeEnd;
            TEvent.Content = PEvent.Content;
            TEvent.Title = PEvent.Title;
            TEvent.GroupId = PEvent.GroupId;
            if (!listDoW.Contains(TEvent.Date.DayOfWeek))
            {
               DeleteTimeEvent(TEvent);
            }
            else
            {
               _dbContext.Entry(TEvent).State = System.Data.Entity.EntityState.Modified;
               _dbContext.SaveChanges();
            }
         }
         #endregion

         #region Insert New Time Event If Not On DB
         //Insert new case, which don't exists on old DB
         for (DateTime date = PEvent.DateStart; date <= PEvent.DateEnd; date = date.AddDays(1))
         {
            if (listDoW.Contains(date.DayOfWeek))
            {
               int result = _dbContext.TimeEvents.Count(n => n.Date.Equals(date));
               if (result == 0)
               {
                  TimeEvent TEvent = new TimeEvent();
                  TEvent.Title = PEvent.Title;
                  TEvent.Content = PEvent.Content;
                  TEvent.CreatedUserId = PEvent.CreatedUserId;
                  TEvent.Date = date;
                  TEvent.TimeStart = PEvent.TimeStart;
                  TEvent.TimeEnd = PEvent.TimeEnd;
                  TEvent.PE_Id = PEvent.Id;
                  TEvent.GroupId = PEvent.GroupId;
                  _dbContext.TimeEvents.Add(TEvent);
                  _dbContext.SaveChanges();

                  AddTEventDetailForGroup(TEvent.Id, PEvent.GroupId);
               }
            }
         }
         #endregion
      }
   }
}