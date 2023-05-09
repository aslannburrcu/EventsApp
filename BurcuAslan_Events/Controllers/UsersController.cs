using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using BurcuAslan_Events.Models;

namespace BurcuAslan_Events.Controllers
{
    public class UsersController : Controller
    {
        EtkinlikAppEntities db = new EtkinlikAppEntities();

        // GET: Users
    
        public async Task<ActionResult> Index()
        {
            return View(await db.Users.ToListAsync());
        }

        // GET: Users/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Users users = db.Users.Find(id);
            if (users == null)
            {
                return HttpNotFound();
            }
            return View(users);
        }

        // GET: Users/Create
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

        // GET: Users/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Users users = db.Users.Find(id);
            if (users == null)
            {
                return HttpNotFound();
            }
            return View(users);
        }

        // POST: Users/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "User_id,User_name,User_lastname,Email,Password,Is_Admin")] Users users)
        {
            if (ModelState.IsValid)
            {
                db.Entry(users).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(users);
        }

        // GET: Users/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Users users = db.Users.Find(id);
            if (users == null)
            {
                return HttpNotFound();
            }
            return View(users);
        }

        // POST: Users/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Users users = db.Users.Find(id);
            db.Users.Remove(users);
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
