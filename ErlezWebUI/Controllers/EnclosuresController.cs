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
    [Authorize]
    public class EnclosuresController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: Enclosures
        public ActionResult Index()
        {
            var enclosures = db.Enclosures.Include(e => e.Head);
            return View(enclosures.ToList());
        }

        // GET: Enclosures/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Enclosure enclosure = db.Enclosures.Find(id);
            if (enclosure == null)
            {
                return HttpNotFound();
            }
            return View(enclosure);
        }

        // GET: Enclosures/Create
        public ActionResult Create()
        {
            ViewBag.PostId = new SelectList(db.Heads, "PostId", "IAID");
            return View();
        }

        // POST: Enclosures/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "PostId,EnclosureCount,MediaType,FileName,FileCreationDate,EnclosedDataFormat,EnclosedData")] Enclosure enclosure)
        {
            if (ModelState.IsValid)
            {
                db.Enclosures.Add(enclosure);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.PostId = new SelectList(db.Heads, "PostId", "IAID", enclosure.PostId);
            return View(enclosure);
        }

        // GET: Enclosures/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Enclosure enclosure = db.Enclosures.Find(id);
            if (enclosure == null)
            {
                return HttpNotFound();
            }
            ViewBag.PostId = new SelectList(db.Heads, "PostId", "IAID", enclosure.PostId);
            return View(enclosure);
        }

        // POST: Enclosures/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "PostId,EnclosureCount,MediaType,FileName,FileCreationDate,EnclosedDataFormat,EnclosedData")] Enclosure enclosure)
        {
            if (ModelState.IsValid)
            {
                db.Entry(enclosure).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.PostId = new SelectList(db.Heads, "PostId", "IAID", enclosure.PostId);
            return View(enclosure);
        }

        // GET: Enclosures/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Enclosure enclosure = db.Enclosures.Find(id);
            if (enclosure == null)
            {
                return HttpNotFound();
            }
            return View(enclosure);
        }

        // POST: Enclosures/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Enclosure enclosure = db.Enclosures.Find(id);
            db.Enclosures.Remove(enclosure);
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
