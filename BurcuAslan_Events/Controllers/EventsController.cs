using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using BurcuAslan_Events.Models;

namespace BurcuAslan_Events.Controllers
{
    public class EventsController : Controller
    {
         EtkinlikAppEntities db = new EtkinlikAppEntities();
        ViewModel vm = new ViewModel();

        // GET: Events
        public ActionResult Index()
        {
            vm.events = db.Events.ToList();
            vm.ticket = db.Tickets.ToList();

            vm.event_categories = db.Event_categories.ToList();
            //var events = db.Events.Include(e => e.Cities).Include(e => e.Event_categories).Include(e => e.Users);

            int checkboxValue = TempData["CheckboxValue"] == null ? 0 : (int)TempData["CheckboxValue"];
            ViewBag.CheckboxValue = checkboxValue;
            return View(vm);
        }


        public ActionResult MyEvents(int? id)
        {
            var user = db.Users.FirstOrDefault(x => x.User_id == id);
            if (user == null) return HttpNotFound(); // kullanıcı bulunamadıysa 404 döndür

            var userEvents = db.Events.Where(e => e.Event_id == user.User_id).ToList();
            return View(userEvents);
        }
        public ActionResult ProfilView(int id)
        {
            var profil = db.Users.Where(x => x.User_id == id).ToList();
            return View(profil);
        }
        public ActionResult TicketBuy(int id)
        {
            var eventInfo = db.Events.SingleOrDefault(x => x.Event_id == id);
            Session["message"] = "Katıldı";

            return RedirectToAction("Index");

            if (eventInfo == null)
            {
                return RedirectToAction("Error");
            }

            eventInfo.Event_statu = !eventInfo.Event_statu;
            db.SaveChanges();

            var buttonText = eventInfo.Event_statu ? "katıldınız" : "katılmadınız";

            ViewBag.ButtonText = buttonText;


           // var Tickets = db.Tickets.Where(x => x.Event_no == id).ToList();
            return View();
        }

        public ActionResult PasswordEditing(int? id)
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
        public ActionResult PasswordEditing([Bind(Include = "User_id,User_name,User_lastname,Email,Password,Is_Admin")] Users users)
        {
            if (ModelState.IsValid)
            {
                db.Entry(users).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(users);
        }


        // GET: Events/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Events events = db.Events.Find(id);
            if (events == null)
            {
                return HttpNotFound();
            }
            return View(events);
        }

        // GET: Events/Create
        public ActionResult Create()
        {
            ViewBag.City_no = new SelectList(db.Cities, "City_id", "City_name");
            ViewBag.Event_cate_no = new SelectList(db.Event_categories, "Event_cate_id", "Event_cate_name");
            ViewBag.User_no = new SelectList(db.Users, "User_id", "User_name");
            return View();
        }

        // POST: Events/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Event_id,Event_name,Event_date,Event_desc,City_no,Event_Adress,Capacity,Event_cate_no,User_no,Event_statu")] Events events)
        {

            if (ModelState.IsValid)
            {
                db.Events.Add(events);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.City_no = new SelectList(db.Cities, "City_id", "City_name", events.City_no);
            ViewBag.Event_cate_no = new SelectList(db.Event_categories, "Event_cate_id", "Event_cate_name", events.Event_cate_no);
            ViewBag.User_no = new SelectList(db.Users, "User_id", "User_name", events.User_no);
            return View(events);
        }

        // GET: Events/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Events events = db.Events.Find(id);
            if (events == null)
            {
                return HttpNotFound();
            }
            ViewBag.City_no = new SelectList(db.Cities, "City_id", "City_name", events.City_no);
            ViewBag.Event_cate_no = new SelectList(db.Event_categories, "Event_cate_id", "Event_cate_name", events.Event_cate_no);
            ViewBag.User_no = new SelectList(db.Users, "User_id", "User_name", events.User_no);
            return View(events);
        }

        // POST: Events/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Event_id,Event_name,Event_date,Event_desc,City_no,Event_Adress,Capacity,Event_cate_no,User_no,Event_statu")] Events events)
        {
            if (ModelState.IsValid)
            {
                db.Entry(events).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.City_no = new SelectList(db.Cities, "City_id", "City_name", events.City_no);
            ViewBag.Event_cate_no = new SelectList(db.Event_categories, "Event_cate_id", "Event_cate_name", events.Event_cate_no);
            ViewBag.User_no = new SelectList(db.Users, "User_id", "User_name", events.User_no);
            return View(events);
        }

        // GET: Events/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Events events = db.Events.Find(id);
            if (events == null)
            {
                return HttpNotFound();
            }
            return View(events);
        }

        // POST: Events/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Events events = db.Events.Find(id);
            db.Events.Remove(events);
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
