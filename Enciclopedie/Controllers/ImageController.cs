using Enciclopedie.Models;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Enciclopedie.Controllers
{
    public class ImageController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        [AllowAnonymous]
        public ActionResult Index()
        {
            var images = db.Images.Include("Article");

            ViewBag.Images = images;

            ViewBag.afisareButoane = false;
            if (User.IsInRole("Editor") || User.IsInRole("Administrator"))
            {
                ViewBag.afisareButoane = true;
            }
            ViewBag.esteAdmin = User.IsInRole("Administrator");

            if (TempData.ContainsKey("message"))
            {
                ViewBag.message = TempData["message"].ToString();
            }

            return View();
        }

        [Authorize(Roles = "User,Editor,Administrator")]
        public ActionResult Show(int id)
        {
            Image image = db.Images.Find(id);
            ViewBag.Image = image;
            ViewBag.Article = image.Article;

            ViewBag.afisareButoane = false;
            if (User.IsInRole("Editor") || User.IsInRole("Administrator"))
            {
                ViewBag.afisareButoane = true;
            }
            ViewBag.esteAdmin = User.IsInRole("Administrator");

            return View(image);
        }

        [Authorize(Roles = "Editor,Administrator")]
        public ActionResult New()
        {
            Image image = new Image();

            // preluam lista de articole din metoda GetAllArticles()
            image.Articles = GetAllArticles();

            return View(image);
        }

        [NonAction]
        public IEnumerable<SelectListItem> GetAllArticles()
        {
            // generam o lista goala
            var selectList = new List<SelectListItem>();
            // Extragem toate articolele din baza de date care sunt disponibile
            var articles = from art in db.Articles.Where(i => i.Available == true)
                             select art;
            // iteram prin articole
            foreach (var article in articles)
            {
                // Adaugam in lista elementele necesare pentru dropdown
                selectList.Add(new SelectListItem
                {
                    Value = article.Id.ToString(),
                    Text = article.Title.ToString()
                });
            }
            // returnam lista de articole
            return selectList;
        }

        [Authorize(Roles = "Editor,Administrator")]
        [HttpPost]
        public ActionResult New(Image image)
        {
            image.Articles = GetAllArticles();

            string fileName = Path.GetFileNameWithoutExtension(image.ImageFile.FileName);
            string extension = Path.GetExtension(image.ImageFile.FileName);
            fileName = fileName + DateTime.Now.ToString("yymmssfff") + extension;
            image.ImagePath = "~/Images/" + fileName;
            fileName = Path.Combine(Server.MapPath("~/Images/"), fileName);
            image.ImageFile.SaveAs(fileName);
       
            try
            {
                if (ModelState.IsValid)
                {
                    Article article = db.Articles.Find(image.Id);
                    article.Available = false;
                    UpdateModel(article);

                    db.Images.Add(image);
                    db.SaveChanges();
                    TempData["message"] = "Imaginea a fost adaugata!";

                    return RedirectToAction("Index");
                }
                else
                {
                    return View(image);
                }
            }
            catch (Exception e)
            {
                return View(image);
            }
        }

        [Authorize(Roles = "Editor,Administrator")]
        public ActionResult Edit(int id)
        {
            Image image = db.Images.Find(id);
            return View(image);
        }

        [Authorize(Roles = "Editor,Administrator")]
        [HttpPut]
        public ActionResult Edit(int id, Image requestImage)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    Image image = db.Images.Find(id);

                    string fileName = Path.GetFileNameWithoutExtension(requestImage.ImageFile.FileName);
                    string extension = Path.GetExtension(requestImage.ImageFile.FileName);
                    fileName = fileName + DateTime.Now.ToString("yymmssfff") + extension;
                    requestImage.ImagePath = "~/Images/" + fileName;
                    fileName = Path.Combine(Server.MapPath("~/Images/"), fileName);
                    requestImage.ImageFile.SaveAs(fileName);

                    if (TryUpdateModel(image))
                    {
                        image.ImageTitle = requestImage.ImageTitle;
                        image.ImagePath = requestImage.ImagePath;
                        image.ImageFile = requestImage.ImageFile;
                        db.SaveChanges();
                        TempData["message"] = "Imaginea a fost modificata!";

                        return RedirectToAction("Index");
                    }
                    return View(requestImage);
                }
                else
                {
                    return View(requestImage);
                }
            }
            catch (Exception e)
            {
                return View(requestImage);
            }
        }

        [Authorize(Roles = "Editor,Administrator")]
        [HttpDelete]
        public ActionResult Delete(int id)
        {
            Article article = db.Articles.Find(id);
            article.Available = true;
            UpdateModel(article);

            Image image = db.Images.Find(id);
            db.Images.Remove(image);
            db.SaveChanges();
            TempData["message"] = "Imaginea a fost stearsa!";
            return RedirectToAction("Index");
        }
    }
}