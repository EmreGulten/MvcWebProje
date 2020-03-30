using System;
using System.Collections.Generic;
using System.Data;
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
    public class SliderController : Controller
    {
        private KurumsalDBContext db = new KurumsalDBContext();

        // GET: Slider
        public ActionResult Index()
        {
            return View(db.Slider.ToList());
        }

        // GET: Slider/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Slider slider = db.Slider.Find(id);
            if (slider == null)
            {
                return HttpNotFound();
            }
            return View(slider);
        }

        // GET: Slider/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Slider/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [ValidateInput(false)]
        public ActionResult Create([Bind(Include = "SliderId,Baslik,Aciklama,ResimURL")] Slider slider, HttpPostedFileBase ResimURL)
        {
            if (ModelState.IsValid)
            {
                if (ResimURL != null)
                {
                    WebImage img = new WebImage(ResimURL.InputStream);
                    FileInfo fileInfo = new FileInfo(ResimURL.FileName);

                    string newFoto = Guid.NewGuid().ToString() + fileInfo.Extension;
                    img.Resize(1024, 360);
                    img.Save("~/Upload/Slider/" + newFoto);
                    slider.ResimURL = "/Upload/Slider/" + newFoto;

                    db.Slider.Add(slider);
                    db.SaveChanges();
                    return RedirectToAction("Index");
                }

            }

            return View(slider);
        }

        // GET: Slider/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Slider slider = db.Slider.Find(id);
            if (slider == null)
            {
                return HttpNotFound();
            }
            return View(slider);
        }

        // POST: Slider/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "SliderId,Baslik,Aciklama,ResimURL")] Slider model, HttpPostedFileBase ResimURL,int id)
        {
            if (ModelState.IsValid)
            {
                var slider = db.Slider.Find(id);

                if (ResimURL != null)
                {
                    if (System.IO.File.Exists(Server.MapPath(slider.ResimURL)))
                    {
                        System.IO.File.Delete(Server.MapPath(slider.ResimURL));
                    }
                    WebImage img = new WebImage(ResimURL.InputStream);
                    FileInfo fileInfo = new FileInfo(ResimURL.FileName);

                    string newFoto = Guid.NewGuid().ToString() + fileInfo.Extension;
                    img.Resize(1024, 360);
                    img.Save("~/Upload/Blog/" + newFoto);
                    model.ResimURL = "/Upload/Blog/" + newFoto;

                }

                slider.Baslik = model.Baslik;
                slider.Aciklama = model.Aciklama;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(model);
        }

        // GET: Slider/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Slider slider = db.Slider.Find(id);
            if (slider == null)
            {
                return HttpNotFound();
            }
            return View(slider);
        }

        // POST: Slider/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Slider slider = db.Slider.Find(id);
            if (System.IO.File.Exists(Server.MapPath(slider.ResimURL)))
            {
                System.IO.File.Delete(Server.MapPath(slider.ResimURL));
            }
            db.Slider.Remove(slider);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
