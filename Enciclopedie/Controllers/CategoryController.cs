using Enciclopedie.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Enciclopedie.Controllers
{
    public class CategoryController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        [AllowAnonymous]
        public ActionResult Index()
        {
            var categories = from category in db.Categories
                           orderby category.CategoryName
                           select category;
            ViewBag.Categories = categories;

            if (TempData.ContainsKey("message"))
            {
                ViewBag.message = TempData["message"].ToString();
            }

            return View();
        }

        public ActionResult Show(int id)
        {
            Category category = db.Categories.Find(id);
            ViewBag.Category = category;

            ViewBag.afisareButoane = false;
            if (User.IsInRole("Administrator"))
            {
                ViewBag.afisareButoane = true;
            }

            return View(category);
        }

        [Authorize(Roles = "Administrator")]
        public ActionResult New()
        {
            return View();
        }

        [Authorize(Roles = "Administrator")]
        [HttpPost]
        public ActionResult New(Category category)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    db.Categories.Add(category);
                    //TryUpdateModel(category);
                    db.SaveChanges();
                    TempData["message"] = "Categoria a fost adaugata!";
                    return RedirectToAction("Index");
                }
                else
                {
                    return View(category);
                }
                
            }
            catch (Exception e)
            {
                return View(category);
            }
        }

        [Authorize(Roles = "Administrator")]
        public ActionResult Edit(int id)
        {
            Category category = db.Categories.Find(id);
            return View(category);
        }

        [Authorize(Roles = "Administrator")]
        [HttpPut]
        public ActionResult Edit(int id, Category requestCategory)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    Category category = db.Categories.Find(id);
                    if (TryUpdateModel(category))
                    {
                        category.CategoryName = requestCategory.CategoryName;
                        db.SaveChanges();
                        TempData["message"] = "Categoria a fost modificata!";
                    }
                    return RedirectToAction("Index");
                }
                else
                {
                    return View(requestCategory);
                }
            }
            catch (Exception e)
            {
                return View(requestCategory);
            }
        }

        [Authorize(Roles = "Administrator")]
        [HttpDelete]
        public ActionResult Delete(int id)
        {
            Category category = db.Categories.Find(id);
            db.Categories.Remove(category);
            db.SaveChanges();
            TempData["message"] = "Categoria a fost stearsa!";
            return RedirectToAction("Index");
        }
    }
}