using Enciclopedie.Models;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Enciclopedie.Controllers
{
    [Authorize(Roles = "Administrator")]
    public class UsersController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();
        public ActionResult Index()
        {
            ViewBag.UsersList = db.Users
                .OrderBy(u => u.UserName)
                .ToList();

            return View();
        }

        public ActionResult Details(string id)
        {
            if (String.IsNullOrEmpty(id))
            {
                return HttpNotFound("Lipseste parametrul id!");
            }

            ApplicationUser user = db.Users
                .Include("Roles")
                .FirstOrDefault(u => u.Id.Equals(id));

            if (user != null)
            {
                ViewBag.UserRole = db.Roles
                    .Find(user.Roles.First().RoleId).Name;

                return View(user);
            }

            return HttpNotFound("Nu s-a gasit user-ul cu id-ul dat!");
        }

        public ActionResult Edit(string id)
        {
            if (String.IsNullOrEmpty(id))
            {
                return HttpNotFound("Lipseste parametrul id!");
            }
            UserViewModel uvm = new UserViewModel();
            uvm.User = db.Users.Find(id);
            IdentityRole userRole = db.Roles
                .Find(uvm.User.Roles.First().RoleId);
            
            uvm.RoleName = userRole.Name;
            
            return View(uvm);
        }

        [HttpPut]
        public ActionResult Edit(string id, UserViewModel uvm)
        {
            ApplicationUser user = db.Users.Find(id);

            try
            {
                if (TryUpdateModel(user))
                {
                    var um = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(db));

                    foreach (var r in db.Roles.ToList())
                    {
                        um.RemoveFromRole(user.Id, r.Name);
                    }

                    um.AddToRole(user.Id, uvm.RoleName);

                    db.SaveChanges();
                }

                return RedirectToAction("Index");
            }
            catch (Exception e)
            {
                return View(uvm);
            }
        }
    }
}