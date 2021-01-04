using Enciclopedie.Models;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Enciclopedie.Controllers
{
    public class ArticleController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        [AllowAnonymous]
        public ActionResult Index()
        {
            var articles = db.Articles.Include("Category").Include("User");

            ViewBag.Articles = articles;

            if (TempData.ContainsKey("message"))
            {
                ViewBag.message = TempData["message"].ToString();
            }

            return View();
        }

        [Authorize(Roles = "User,Editor,Administrator")]
        public ActionResult Show(int id)
        {
            Article article = db.Articles.Find(id);
            ViewBag.Article = article;
            ViewBag.Category = article.Category;

            ViewBag.afisareButoane = false;
            if (User.IsInRole("Editor") || User.IsInRole("Administrator"))
            {
                ViewBag.afisareButoane = true;
            }
            ViewBag.esteAdmin = User.IsInRole("Administrator");
            ViewBag.utilizatorCurent = User.Identity.GetUserId();

            return View(article);
        }

        [Authorize(Roles = "Editor,Administrator")]
        public ActionResult New()
        {
            Article article = new Article();

            // preluam lista de categorii din metoda GetAllCategories()
            article.Categories = GetAllCategories();

            // Preluam ID-ul utilizatorului curent
            article.UserId = User.Identity.GetUserId();

            return View(article);
        }

        [NonAction]
        public IEnumerable<SelectListItem> GetAllCategories()
        {
            var selectList = new List<SelectListItem>();
           
            var categories = from cat in db.Categories
                             select cat;
           
            foreach (var category in categories)
            {
                selectList.Add(new SelectListItem
                {
                    Value = category.CategoryId.ToString(),
                    Text = category.CategoryName.ToString()
                });
            }
            
            return selectList;
        }

        [Authorize(Roles = "Editor,Administrator")]
        [HttpPost]
        public ActionResult New(Article article)
        {
            article.Categories = GetAllCategories();
            try
            {
                if (ModelState.IsValid)
                {
                    db.Articles.Add(article);
                    db.SaveChanges();
                    TempData["message"] = "Articolul a fost adaugat!";
                    return RedirectToAction("Index");
                }
                else
                {
                    return View(article);
                }
            }
            catch (Exception e)
            {
                return View(article);
            }
        }

        [Authorize(Roles = "Editor,Administrator")]
        public ActionResult Edit(int id)
        {
            Article article = db.Articles.Find(id);
            article.Categories = GetAllCategories();

            if (article.UserId == User.Identity.GetUserId() || User.IsInRole("Administrator"))
            {
                return View(article);
            }
            else
            {
                TempData["message"] = "Nu aveti dreptul sa faceti modificari asupra unui articol care nu va apartine!";
                return RedirectToAction("Index");
            }
        }

        [Authorize(Roles = "Editor,Administrator")]
        [HttpPut]
        public ActionResult Edit(int id, Article requestArticle)
        {
            requestArticle.Categories = GetAllCategories();
            try
            {
                if (ModelState.IsValid)
                {
                    Article article = db.Articles.Find(id);
                    if (article.UserId == User.Identity.GetUserId() || User.IsInRole("Administrator"))
                    {
                        if (TryUpdateModel(article))
                        {
                            article.Title = requestArticle.Title;
                            article.Summary = requestArticle.Summary;
                            article.Content = requestArticle.Content;
                            article.Date = requestArticle.Date;
                            article.CategoryId = requestArticle.CategoryId;
                            db.SaveChanges();
                            TempData["message"] = "Articolul a fost modificat!";
                        }
                        return RedirectToAction("Index");
                    }
                    else
                    {
                        TempData["message"] = "Nu aveti dreptul sa faceti modificari asupra unui articol care nu va apartine!";
                        return RedirectToAction("Index");
                    }
                }
                else
                {
                    return View(requestArticle);
                }
            }
            catch (Exception e)
            {
                return View(requestArticle);
            }
        }

        [Authorize(Roles = "Editor,Administrator")]
        [HttpDelete]
        public ActionResult Delete(int id)
        {
            Article article = db.Articles.Find(id);
            Image image = db.Images.Find(id);
            if (article.UserId == User.Identity.GetUserId() || User.IsInRole("Administrator"))
            {
                if(image!=null)
                {
                    db.Images.Remove(image);
                }
              
                db.Articles.Remove(article);
                db.SaveChanges();
                TempData["message"] = "Articolul a fost sters!";
                return RedirectToAction("Index");
            }
            else
            {
                TempData["message"] = "Nu aveti dreptul sa stergeti un articol care nu va apartine!";
                return RedirectToAction("Index");
            }
        }
    }
}
