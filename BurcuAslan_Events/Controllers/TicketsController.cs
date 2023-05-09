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
    public class TicketsController : Controller
    {
         EtkinlikAppEntities db = new EtkinlikAppEntities();

        // GET: Tickets
        public ActionResult Index()
        {
            var tickets = db.Tickets.Include(t => t.Events).Include(t => t.Users);
            return View(tickets.ToList());
        }

        // GET: Tickets/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Tickets tickets = db.Tickets.Find(id);
            if (tickets == null)
            {
                return HttpNotFound();
            }
            return View(tickets);
        }

        // GET: Tickets/Create
        public ActionResult Create()
        {
          

            ViewBag.Event_no = new SelectList(db.Events, "Event_id", "Event_name");
            ViewBag.User_no = new SelectList(db.Users, "User_id", "User_name");
            return View();
        }

        // POST: Tickets/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Ticket_code,Event_no,User_no,Attendance_status ")] Tickets tickets)
        {

             
            if (ModelState.IsValid)
            {
                db.Tickets.Add(tickets);
                db.SaveChanges();

                Session["message"] = "Katıldı";

                return RedirectToAction("Index");
            }

            ViewBag.Event_no = new SelectList(db.Events, "Event_id", "Event_name", tickets.Event_no);
            ViewBag.User_no = new SelectList(db.Users, "User_id", "User_name", tickets.User_no);
            return View(tickets);
        }

        // GET: Tickets/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Tickets tickets = db.Tickets.Find(id);
            if (tickets == null)
            {
                return HttpNotFound();
            }
            ViewBag.Event_no = new SelectList(db.Events, "Event_id", "Event_name", tickets.Event_no);
            ViewBag.User_no = new SelectList(db.Users, "User_id", "User_name", tickets.User_no);
            return View(tickets);
        }

        // POST: Tickets/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Ticket_code,Event_no,User_no,Attendance_status")] Tickets tickets)
        {
            if (ModelState.IsValid)
            {
                db.Entry(tickets).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.Event_no = new SelectList(db.Events, "Event_id", "Event_name", tickets.Event_no);
            ViewBag.User_no = new SelectList(db.Users, "User_id", "User_name", tickets.User_no);
            return View(tickets);
        }

        // GET: Tickets/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Tickets tickets = db.Tickets.Find(id);
            if (tickets == null)
            {
                return HttpNotFound();
            }
            return View(tickets);
        }

        // POST: Tickets/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Tickets tickets = db.Tickets.Find(id);
            db.Tickets.Remove(tickets);
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
