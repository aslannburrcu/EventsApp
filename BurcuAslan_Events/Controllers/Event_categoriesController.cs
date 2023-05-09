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
    public class Event_categoriesController : Controller
    {
        EtkinlikAppEntities db = new EtkinlikAppEntities();
        ViewModel vm = new ViewModel();

        // GET: Event_categories
        public ActionResult Index()
        {
            vm.cities = db.Cities.ToList();
            vm.event_categories = db.Event_categories.ToList();
            return View(vm);
        }

        // GET: Event_categories/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Event_categories event_categories = db.Event_categories.Find(id);
            if (event_categories == null)
            {
                return HttpNotFound();
            }
            return View(event_categories);
        }

        // GET: Event_categories/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Event_categories/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Event_cate_id,Event_cate_name,City_id,City_name")] Event_categories event_categories)
        {
            if (ModelState.IsValid)
            {
               
                db.Event_categories.Add(event_categories);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(event_categories);
        }

        // GET: Event_categories/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Event_categories event_categories = db.Event_categories.Find(id);
            if (event_categories == null)
            {
                return HttpNotFound();
            }
            return View(event_categories);
        }

        // POST: Event_categories/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Event_cate_id,Event_cate_name")] Event_categories event_categories)
        {
            if (ModelState.IsValid)
            {
                db.Entry(event_categories).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(event_categories);
        }

        // GET: Event_categories/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Event_categories event_categories = db.Event_categories.Find(id);
            if (event_categories == null)
            {
                return HttpNotFound();
            }
            return View(event_categories);
        }

        // POST: Event_categories/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Event_categories event_categories = db.Event_categories.Find(id);
            db.Event_categories.Remove(event_categories);
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
