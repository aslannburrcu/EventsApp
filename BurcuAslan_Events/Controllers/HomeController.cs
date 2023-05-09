using BurcuAslan_Events.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace BurcuAslan_Events.Controllers
{
     
    public class HomeController : Controller
    {
        EtkinlikAppEntities db = new EtkinlikAppEntities();
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult About()
        {

            ViewBag.Message = "Profil bilgilaeriniz aşağıdadır.";

            return View();
        }



        public ActionResult Login()
        {

            return View();
        }
        [HttpPost]
        public async Task<ActionResult> Login(Users user, Admin admin)
        {
            string message = "";
          var users=  db.Users.FirstOrDefault(x => x.User_name == user.User_name && x.Password == user.Password);
           var admins= db.Admin.FirstOrDefault(x => x.Admin_name == admin.Admin_name && x.Password == admin.Password);
            if (user.Is_Admin == false && user != null)
            {
                Session["user"] = user;
                return RedirectToAction("Index", "Events");
            }
            else
                return RedirectToAction("Index", "EventsDecisions");

        }
        public ActionResult Create()
        {
            return View();
        }

        // POST: Users/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "User_id,User_name,User_lastname,Email,Password,Is_Admin")] Users users)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    db.Users.Add(users);
                    db.SaveChanges();
                    ViewBag.kayit_durum = "Kayıt Başarılı";
                }
                catch (Exception hata)
                {
                    int yeri = hata.InnerException.ToString().IndexOf("uk_uyeler_email");//-1 yok demekki kuladi hatası
                    if (yeri != -1)
                        ViewBag.kayit_durum = "Böyle Email var Kayıt Yapılamıyor";
                    else
                    {
                        ViewBag.kayit_durum = "Böyle kuladi var değiştirin var Kayıt Yapılamıyor";
                        List<string> kuladi_onerilerim = kullanici_adi_oner(users.User_name);
                        ViewBag.kuladi_onerilerim = kuladi_onerilerim;
                    }


                }
                return RedirectToAction("Index", "Users");
            }//if

            return View(users);
        }

        List<string> kuladi_oneri_listemiz = new List<string>();
        byte sayac = 0;
        List<string> kullanici_adi_oner(string kuladi)
        {
            System.Threading.Thread.Sleep(500);
            string[] harfler = { "a", "p", "b", "c", "t" };
            string oneri_kuladi = kuladi + new Random().Next(0, 9) + harfler[new Random().Next(0, 4)];
            int varmi = db.Users.Where(x => x.User_name == oneri_kuladi).Count();
            if (varmi == 0)//demekki vt de yok
            {
                kuladi_oneri_listemiz.Add(oneri_kuladi);
                sayac++;
            }
            if (sayac != 5) kullanici_adi_oner(kuladi);
            return kuladi_oneri_listemiz;
        }
    }
}