using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Web;
using System.Web.Helpers;
using System.Web.Mvc;
using MvcWebProje.Models;
using MvcWebProje.Models.DataContext;
using MvcWebProje.Models.Model;

namespace MvcWebProje.Controllers
{
    public class AdminController : Controller
    {
        KurumsalDBContext db = new KurumsalDBContext();
        // GET: Admin
        public ActionResult Index()
        {
            ViewBag.kimlik = db.Kimlik.SingleOrDefault();
            ViewBag.Yorum = db.Yorum.Where(x => x.Onay == false).Count();


            ViewBag.BlogSay = db.Blog.Count();
            ViewBag.KategoriSay = db.Kategori.Count();
            ViewBag.HizmetSay = db.Hizmet.Count();
            ViewBag.YorumSay = db.Yorum.Count();



            var sorgu = db.Kategori.ToList();
            return View(sorgu);
        }

        public ActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Login(Admin model/*,string sifre*/)
        {
            //var md5pass = Crypto.Hash(sifre, "MD5");
            var login = db.Admin.SingleOrDefault(x => x.Eposta == model.Eposta);

            if (login.Eposta == model.Eposta && login.Sifre == Crypto.Hash(model.Sifre, "MD5"))
            {
                Session["adminid"] = login.AdminId;
                Session["eposta"] = login.Eposta;
                Session["yetki"] = login.Yetki;
                return RedirectToAction("Index");
            }

            ViewBag.uyari = "Kullanici Adı yada Şifre Yanlış";

            return View(model);
        }

        public ActionResult Logout()
        {
            Session["adminid"] = null;
            Session["eposta"] = null;

            Session.Abandon();
            return RedirectToAction("Login", "Admin");
        }

        public ActionResult SifremiUnuttum()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin")]
        public ActionResult SifremiUnuttum(string eposta)
        {
            var mail = db.Admin.SingleOrDefault(x => x.Eposta == eposta);

            if (mail != null)
            {
                System.Threading.Thread.CurrentThread.CurrentUICulture = new System.Globalization.CultureInfo("En");

                Random rnd = new Random();
                int yeniSifre = rnd.Next();

                mail.Sifre = Crypto.Hash(yeniSifre.ToString(), "MD5");
                db.SaveChanges();

                var senderEmail = new MailAddress("fatal-ryhmer@hotmail.com", "");
                var receivereEmail = new MailAddress(eposta, "Receiver");
                var password = "e886179+-";
                var sub = "Admin Paneli Giriş Şifreniz";
                var body = "Şifreniz : " + yeniSifre.ToString();
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

                ViewBag.D = "a";
                ViewBag.uyari = "Şifreniz Gönderildi. Teşekkürler!";

            }
            else
            {
                ViewBag.uyari = "Bir Hata Oluştu";
            }

            return View();
        }

        public ActionResult YorumOnay()
        {
            var yorum = db.Yorum.ToList();
            ViewBag.kimlik = db.Kimlik.SingleOrDefault();
            return View(yorum);
        }

        public ActionResult Adminler()
        {

            return View(db.Admin.ToList());
        }

        public ActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin")]
        public ActionResult Create(Admin model, string Sifre, string Eposta)
        {
            if (ModelState.IsValid)
            {
                model.Sifre = Crypto.Hash(Sifre, "MD5");
                db.Admin.Add(model);
                db.SaveChanges();
                return RedirectToAction("Adminler");
            }
            return View(model);
        }


        public ActionResult Edit(int? id)
        {
            var admin = db.Admin.Find(id);

            if (admin == null)
            {
                return HttpNotFound();
            }

            return View(admin);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin")]
        public ActionResult Edit(Admin model, string Eposta, string Sifre, int id)
        {
            if (ModelState.IsValid)
            {
                var admin = db.Admin.Find(id);

                if (admin != null)
                {
                    admin.Eposta = model.Eposta;
                    admin.Yetki = model.Yetki;
                    admin.Sifre = Crypto.Hash(Sifre, "MD5");
                    db.SaveChanges();
                    return RedirectToAction("Adminler");
                }
            }

            return View(model);
        }

        [HttpPost]
        [Authorize(Roles = "Admin")]
        public JsonResult Delete(int id)
        {
            bool result = false;

            var admins = db.Admin.Find(id);

            if (admins != null)
            {
                db.Admin.Remove(admins);
                db.SaveChanges();
                result = true;
            }

            return Json(result, JsonRequestBehavior.AllowGet);

        }
    }

}
