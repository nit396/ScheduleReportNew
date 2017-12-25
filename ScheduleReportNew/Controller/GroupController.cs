using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Microsoft.AspNet.Identity;
using ScheduleReport.Controllers;
using ScheduleReport.Models;
using ScheduleReport.ViewModels;

namespace ScheduleReportNew.Controllers
{
   public class GroupController : Controller
   {
      private AppDbContext _dbContext;
      public GroupController()
      {
         _dbContext = new AppDbContext();
      }
      // GET: Group
      public ActionResult Index()
      {
         if(!Request.IsAuthenticated)
         {
            return RedirectToAction("Login", "Account");
         }

         string userId = User.Identity.GetUserId();
         List<Group> listGroup = _dbContext.Groups.Where(n => n.CreatedUserId.Equals(userId)).ToList<Group>();

         return View(listGroup);
      }

      [HttpGet]
      public ActionResult Create()
      {
         if (!Request.IsAuthenticated)
         {
            return RedirectToAction("Login", "Account");
         }

         return PartialView();
      }

      [HttpPost]
      public ActionResult Create(GroupViewModel viewModel)
      {
         if (!Request.IsAuthenticated)
         {
            return RedirectToAction("Login", "Account");
         }

         string userId = User.Identity.GetUserId();

         try
         {
            Group group = new Group();
            group.CreatedUserId = userId;
            group.Date = DateTime.Now;
            group.Name = viewModel.Name;

            _dbContext.Groups.Add(group);
            _dbContext.SaveChanges();

            TempData["GroupMessage"] = "Create group " + viewModel.Name + " successfully!";
         }
         catch(Exception ex)
         {
            TempData["GroupMessage"] = "Some error, please contact Admin!";
         }

         return RedirectToAction("Index", "Group");
      }

      [HttpGet]
      public ActionResult Edit(int groupId)
      {
         if (!Request.IsAuthenticated)
         {
            return RedirectToAction("Login", "Account");
         }

         var group = _dbContext.Groups.FirstOrDefault(n => n.Id == groupId);

         GroupViewModel viewModel = new GroupViewModel();
         viewModel.Name = group.Name;
         viewModel.Date = group.Date;
         viewModel.Id = group.Id;

         return PartialView(viewModel);
      }

      [HttpPost]
      public ActionResult Edit(int groupId, GroupViewModel viewModel)
      {
         if (!Request.IsAuthenticated)
         {
            return RedirectToAction("Login", "Account");
         }

         var group = _dbContext.Groups.FirstOrDefault(n => n.Id == groupId);
         group.Name = viewModel.Name;

         _dbContext.Entry(group).State = System.Data.Entity.EntityState.Modified;
         _dbContext.SaveChanges();

         return RedirectToAction("Index", "Group");
      }

      [HttpGet]
      public ActionResult AddMember(int groupId)
      {
         if (!Request.IsAuthenticated)
         {
            return RedirectToAction("Login", "Account");
         }

         ViewBag.GroupId = groupId;
         return PartialView();
      }


      [HttpPost]
      public ActionResult AddMember(int groupId, UserViewModel viewModel)
      {
         if (!Request.IsAuthenticated)
         {
            return RedirectToAction("Login", "Account");
         }

         ViewBag.GroupId = groupId;

         string nowUserId = User.Identity.GetUserId();

         User user;
         if (!_dbContext.Users.Any(n=>n.UserName.Equals(viewModel.UserName)))
         {
            user = new ScheduleReport.Models.User();
            user.CreatedUserId = nowUserId;
            user.FirstName = viewModel.FirstName;
            user.LastName = viewModel.LastName;
            user.UserName = viewModel.UserName;
            user.PasswordHash = new PasswordHasher().HashPassword(viewModel.Password);
            user.SecurityStamp = Guid.NewGuid().ToString();
            user.Type = 2;
            _dbContext.Users.Add(user);
            _dbContext.SaveChanges();
         }
         else user = _dbContext.Users.FirstOrDefault(n => n.UserName.Equals(viewModel.UserName));

         var count = _dbContext.GroupDetails.Count(n => n.GroupId == groupId && n.MemberUserId.Equals(user.Id));
         if(count == 0)
         {
            GroupDetail groupDetail = new GroupDetail();
            groupDetail.GroupId = groupId;
            groupDetail.MemberUserId = user.Id;

            _dbContext.GroupDetails.Add(groupDetail);
            _dbContext.SaveChanges();
         }

         return RedirectToAction("Index", "Group");
      }

      public ActionResult DeleteGroup(int groupId)
      {
         if (!Request.IsAuthenticated)
         {
            return RedirectToAction("Login", "Account");
         }

         var group = _dbContext.Groups.FirstOrDefault(n => n.Id == groupId);
         _dbContext.Entry(group).State = System.Data.Entity.EntityState.Deleted;
         _dbContext.SaveChanges();

         var listGroupDetail = _dbContext.GroupDetails.Where(n => n.GroupId == groupId).ToList<GroupDetail>();
         foreach(var groupDetail in listGroupDetail)
         {
            _dbContext.Entry(groupDetail).State = System.Data.Entity.EntityState.Deleted;
            _dbContext.SaveChanges();
         }

         TempData["GroupMessage"] = "Delete group " + group.Name + " successfully!";
         
         return RedirectToAction("Index", "Group");
      }

      public ActionResult DeleteMember(int groupId, string userId)
      {
         if (!Request.IsAuthenticated)
         {
            return RedirectToAction("Login", "Account");
         }

         var groupDetail = _dbContext.GroupDetails.FirstOrDefault(n => n.GroupId == groupId && n.MemberUserId.Equals(userId));
         _dbContext.Entry(groupDetail).State = System.Data.Entity.EntityState.Deleted;
         _dbContext.SaveChanges();

         var user = _dbContext.Users.FirstOrDefault(n => n.Id.Equals(userId));
         _dbContext.Entry(user).State = System.Data.Entity.EntityState.Deleted;
         _dbContext.SaveChanges();

         TempData["EditGroup"] = groupId;

         return RedirectToAction("Index", "Group");
      }
   }
}