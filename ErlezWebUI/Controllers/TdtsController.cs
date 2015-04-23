using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using ErlezWebUI.Models;

namespace ErlezWebUI.Controllers
{
    public class TdtsController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: Tdts
        public ActionResult Index()
        {
            var tdts = db.Tdts.Include(t => t.Head);
            return View(tdts.ToList());
        }

        // GET: Tdts/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Tdt tdt = db.Tdts.Find(id);
            if (tdt == null)
            {
                return HttpNotFound();
            }
            return View(tdt);
        }

        // GET: Tdts/Create
        public ActionResult Create()
        {
            ViewBag.PostId = new SelectList(db.Heads, "PostId", "IAID");
            return View();
        }

        // POST: Tdts/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "PostId,TdtCount,TdtMethod,TdtDescription,TdtCarrierName")] Tdt tdt)
        {
            if (ModelState.IsValid)
            {
                db.Tdts.Add(tdt);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.PostId = new SelectList(db.Heads, "PostId", "IAID", tdt.PostId);
            return View(tdt);
        }

        // GET: Tdts/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Tdt tdt = db.Tdts.Find(id);
            if (tdt == null)
            {
                return HttpNotFound();
            }
            ViewBag.PostId = new SelectList(db.Heads, "PostId", "IAID", tdt.PostId);
            return View(tdt);
        }

        // POST: Tdts/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "PostId,TdtCount,TdtMethod,TdtDescription,TdtCarrierName")] Tdt tdt)
        {
            if (ModelState.IsValid)
            {
                db.Entry(tdt).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.PostId = new SelectList(db.Heads, "PostId", "IAID", tdt.PostId);
            return View(tdt);
        }

        // GET: Tdts/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Tdt tdt = db.Tdts.Find(id);
            if (tdt == null)
            {
                return HttpNotFound();
            }
            return View(tdt);
        }

        // POST: Tdts/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Tdt tdt = db.Tdts.Find(id);
            db.Tdts.Remove(tdt);
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
