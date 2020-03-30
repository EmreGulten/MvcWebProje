using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Helpers;
using System.Web.Mvc;
using MvcWebProje.Models.DataContext;
using MvcWebProje.Models.Model;

namespace MvcWebProje.Controllers
{
    public class BlogController : Controller
    {
        private KurumsalDBContext db = new KurumsalDBContext();
        // GET: Blog
        public ActionResult Index()
        {
            db.Configuration.LazyLoadingEnabled = false;
            return View(db.Blog.Include("Kategori").ToList().OrderByDescending(x => x.BlogId));
        }

        [HttpGet]
        public ActionResult Create()
        {
            ViewBag.KategoriId = new SelectList(db.Kategori, "KategoriId", "KategoriAd");
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [ValidateInput(false)]
        public ActionResult Create(Blog model, HttpPostedFileBase ResimURL)
        {
            if (ModelState.IsValid)
            {
                if (ResimURL != null)
                {
                    WebImage img = new WebImage(ResimURL.InputStream);
                    FileInfo fileInfo = new FileInfo(ResimURL.FileName);

                    string newFoto = Guid.NewGuid().ToString() + fileInfo.Extension;
                    img.Resize(800,450);
                    img.Save("~/Upload/Blog/" + newFoto);
                    model.ResimURL = "/Upload/Blog/" + newFoto;

                    db.Blog.Add(model);
                    db.SaveChanges();
                    return RedirectToAction("Index");
                }
            }

            return View(model);
        }

        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            var blog = db.Blog.Find(id);

            if (blog == null)
            {
                return HttpNotFound();
            }
            ViewBag.KategoriId = new SelectList(db.Kategori, "KategoriId", "KategoriAd");
            return View(blog);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [ValidateInput(false)]
        public ActionResult Edit(int id, Blog model, HttpPostedFileBase ResimURL)
        {
            if (ModelState.IsValid)
            {
                var blog = db.Blog.Find(id);
                if (ResimURL != null)
                {
                    if (System.IO.File.Exists(Server.MapPath(blog.ResimURL)))
                    {
                        System.IO.File.Delete(Server.MapPath(blog.ResimURL));
                    }
                    WebImage img = new WebImage(ResimURL.InputStream);
                    FileInfo fileInfo = new FileInfo(ResimURL.FileName);

                    string newFoto = Guid.NewGuid().ToString() + fileInfo.Extension;
                    img.Resize(800, 450);
                    img.Save("~/Upload/Blog/" + newFoto);
                    model.ResimURL = "/Upload/Blog/" + newFoto;


                    blog.Baslik = model.Baslik;
                    blog.Icerik = model.Icerik;
                    blog.KategoriId = model.KategoriId;

                    db.SaveChanges();
                    return RedirectToAction("Index");

                }
            }

            return View(model);
        }

        [HttpPost]
        public JsonResult Delete(int id)
        {
            bool result = false;

            var blog = db.Blog.Find(id);

            if (blog != null)
            {
                db.Blog.Remove(blog);
                db.SaveChanges();
                result = true;
            }

            return Json(result, JsonRequestBehavior.AllowGet);

        }
    }
}