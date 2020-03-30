using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using MvcWebProje.Models.DataContext;
using MvcWebProje.Models.Model;

namespace MvcWebProje.Controllers
{
    public class HakkimizdaController : Controller
    {
        KurumsalDBContext db = new KurumsalDBContext();
        // GET: Hakkimizda
        public ActionResult Index()
        {
            return View(db.Hakkimizda.ToList());
        }

        public ActionResult Edit(int id)
        {
            var hak = db.Hakkimizda.SingleOrDefault(x => x.HakkimizdaId == id);
            if (hak == null)
            {
                return HttpNotFound();
            }

            return View(hak);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [ValidateInput(false)]
        public ActionResult Edit(int id , Hakkimizda model)
        {
            if (ModelState.IsValid)
            {
                var hak = db.Hakkimizda.SingleOrDefault(x => x.HakkimizdaId == id);

                if (hak != null)
                {
                    hak.Aciklama = model.Aciklama;
                    db.SaveChanges();
                    return RedirectToAction("Index");
                }
            }

            return View(model);
        }
    }
}