using System;
using System.Collections.Generic;
using System.Data.Entity;
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
    public class HizmetController : Controller
    {
        private KurumsalDBContext db = new KurumsalDBContext();
        // GET: Hizmet
        public ActionResult Index()
        {
            return View(db.Hizmet.ToList());
        }

        public ActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [ValidateInput(false)]
        public ActionResult Create(Hizmet model,HttpPostedFileBase ResimURL)
        {
            if (ModelState.IsValid)
            {
                if (ResimURL != null)
                {
                    WebImage img = new WebImage(ResimURL.InputStream);
                    FileInfo fileInfo = new FileInfo(ResimURL.FileName);

                    string newFoto = Guid.NewGuid().ToString() + fileInfo.Extension;
                    img.Resize(800,600);
                    img.Save("~/Upload/Hizmet/" + newFoto);
                    model.ResimURL = "/Upload/Hizmet/" + newFoto;


                    db.Hizmet.Add(model);
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
                return  new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            var hizmet = db.Hizmet.Find(id);

            if (hizmet == null)
            {
                return HttpNotFound();
            }

            return View(hizmet);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [ValidateInput(false)]
        public ActionResult Edit(int id,Hizmet model,HttpPostedFileBase ResimURL)
        {
            if (ModelState.IsValid)
            {
                var hizmet = db.Hizmet.Find(id);

                if (ResimURL != null)
                {
                    if (System.IO.File.Exists(Server.MapPath(hizmet.ResimURL)))
                    {
                        System.IO.File.Delete(Server.MapPath(hizmet.ResimURL));
                    }
                    WebImage img = new WebImage(ResimURL.InputStream);
                    FileInfo fileInfo = new FileInfo(ResimURL.FileName);

                    string newFoto = Guid.NewGuid().ToString() + fileInfo.Extension;
                    img.Resize(800, 600);
                    img.Save("~/Upload/Hizmet/" + newFoto);
                    model.ResimURL = "/Upload/Hizmet/" + newFoto;

                    hizmet.Aciklama = model.Aciklama;
                    hizmet.Baslik = model.Baslik;
                    db.SaveChanges();
                    return RedirectToAction("Index");
                }
            }

            return View(model);
        }

        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            var hizmet = db.Hizmet.Find(id);

            if (hizmet == null)
            {
                return HttpNotFound();
            }

            db.Hizmet.Remove(hizmet);
            db.SaveChanges();

            return RedirectToAction("Index");
        }




    }
}