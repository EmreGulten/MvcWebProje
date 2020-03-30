using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Web;
using System.Web.Helpers;
using System.Web.Mvc;
using MvcWebProje.Models.DataContext;
using MvcWebProje.Models.Model;
using PagedList;
using PagedList.Mvc;

namespace MvcWebProje.Controllers
{
    public class HomeController : Controller
    {
        private KurumsalDBContext db = new KurumsalDBContext();
        // GET: Home
        public ActionResult Index()
        {
            ViewBag.kimlik = db.Kimlik.SingleOrDefault();

            ViewBag.iletisim = db.Iletisim.SingleOrDefault();

            return View();
        }

        public ActionResult SliderPartial()
        {
            return View(db.Slider.ToList().OrderByDescending(x => x.SliderId));
        }

        public ActionResult HizmetPartial()
        {
            ViewBag.kimlik = db.Kimlik.SingleOrDefault();
            return View(db.Hizmet.ToList().OrderByDescending(x => x.HizmetId));
        }

        public ActionResult Hakkimizda()
        {
            ViewBag.kimlik = db.Kimlik.SingleOrDefault();
            return View(db.Hakkimizda.SingleOrDefault());
        }

        public ActionResult FooterPartial()
        {
            ViewBag.iletisim = db.Iletisim.SingleOrDefault();
            ViewBag.Blog = db.Blog.ToList().OrderByDescending(x => x.BlogId);
            ViewBag.Hizmetler = db.Hizmet.ToList().OrderByDescending(x => x.HizmetId);

            return PartialView();
        }

        public ActionResult BlogKategoriPartial()
        {
            //return PartialView(db.Kategori.ToList().OrderBy(x => x.KategoriAd));
            return PartialView(db.Kategori.Include("Blogs").ToList().OrderBy(x => x.KategoriAd));
        }

        public ActionResult BlogPostPartial()
        {
           
            return PartialView(db.Blog.ToList().OrderByDescending(x=>x.BlogId));
        }

        public ActionResult KategoriBlog(int id, int Sayfa = 1)
        {
            var blog = db.Blog.Include("Kategori").Where(x => x.KategoriId == id).OrderByDescending(x=>x.KategoriId).ToPagedList(Sayfa,5);
            return View(blog);
        }

        public ActionResult Hizmetlerimiz()
        {
            ViewBag.kimlik = db.Kimlik.SingleOrDefault();
            return View(db.Hizmet.ToList().OrderByDescending(x => x.HizmetId));
        }


        public ActionResult Iletisim()
        {
            ViewBag.kimlik = db.Kimlik.SingleOrDefault();
            return View(db.Iletisim.SingleOrDefault());
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Iletisim(string adsoyad = null, string email = null, string konu = null, string mesaj = null)
        {
            if (adsoyad != null && email != null)
            {
                System.Threading.Thread.CurrentThread.CurrentUICulture = new System.Globalization.CultureInfo("En");
                
                var senderEmail = new MailAddress("fatal-ryhmer@hotmail.com", "");
                var receivereEmail = new MailAddress(email, "Receiver");
                var password = "e886179+-";
                var sub = konu;
                var body = mesaj;
                var smtp = new SmtpClient
                {
                    Host = "smtp.live.com",
                    Port = 587,
                    EnableSsl = true,
                    DeliveryMethod = SmtpDeliveryMethod.Network,
                    UseDefaultCredentials = false,
                    Credentials = new NetworkCredential(senderEmail.Address, password)
                };
                using (var mess = new MailMessage(senderEmail, receivereEmail)
                {
                    Subject = sub,
                    Body = body
                })
                {
                    smtp.Send(mess);
                }


                ViewBag.uyari = "Mesajınız Gönderildi. Teşekkürler!";

            }
            else
            {
                ViewBag.uyari = "Bir Hata Oluştu";
            }

            return View();
        }

        public ActionResult Blog(int Sayfa = 1)
        {
            ViewBag.kimlik = db.Kimlik.SingleOrDefault();
            return View(db.Blog.Include("Kategori").OrderByDescending(x => x.BlogId).ToPagedList(Sayfa,5));
        }

        public ActionResult BlogDetay(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            var blog = db.Blog.Include("Kategori").Include("Yorums").SingleOrDefault(x=>x.BlogId==id);

            if (blog == null)
            {
                return HttpNotFound();
            }

            return View(blog);
        }

        public JsonResult Yorum(string AdSoyad, string Eposta, string Icerik, int blogid)
        {
            if (Icerik == null)
            {
                return Json(true, JsonRequestBehavior.AllowGet);
            }

            db.Yorum.Add(new Yorum
            {
                AdSoyad = AdSoyad,
                Eposta = Eposta,
                Icerik = Icerik,
                BlogId = blogid,
                Onay = false
            });

            db.SaveChanges();

            return Json(false, JsonRequestBehavior.AllowGet);
        }

    }
}