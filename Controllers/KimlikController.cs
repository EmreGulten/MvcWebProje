using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Helpers;
using System.Web.Mvc;
using MvcWebProje.Models.DataContext;
using MvcWebProje.Models.Model;

namespace MvcWebProje.Controllers
{
    [Authorize(Roles = "Admin")]
    public class KimlikController : Controller
    {
        KurumsalDBContext db = new KurumsalDBContext();

        // GET: Kimlik
        public ActionResult Index()
        {
            return View(db.Kimlik.ToList());
        }

  

        // GET: Kimlik/Edit/5
        public ActionResult Edit(int id)
        {
            var kimlik = db.Kimlik.SingleOrDefault(x => x.KimlikId == id);
            if (kimlik == null)
            {
                return HttpNotFound();
            }

            return View(kimlik);
        }

        // POST: Kimlik/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        [ValidateInput(false)]
        public ActionResult Edit(int id, Kimlik model,HttpPostedFileBase LogoURL)
        {
            try
            {
                var kimlik = db.Kimlik.SingleOrDefault(x => x.KimlikId == id);
                if (LogoURL !=null)
                {
                    if (System.IO.File.Exists(Server.MapPath(kimlik.LogoURL)))
                    {
                        System.IO.File.Delete(Server.MapPath(kimlik.LogoURL));
                    }

                    WebImage img = new WebImage(LogoURL.InputStream);
                    FileInfo logInfo = new FileInfo(LogoURL.FileName);

                    string newFoto =LogoURL.FileName.ToString() + logInfo.Extension;
                    img.Resize(800, 350);
                    img.Save("~/Upload/Kimlik/" + newFoto);

                    kimlik.Title = model.Title;
                    kimlik.Keywords = model.Keywords;
                    kimlik.Description = model.Description;
                    kimlik.LogoURL = "/Upload/Kimlik/" + newFoto;
                    kimlik.Unvan = model.Unvan;

                    db.SaveChanges();

                    return RedirectToAction("Index");

                }

                return View();
            }
            catch
            {
                return View(model);
            }
        }

     
    }
}
