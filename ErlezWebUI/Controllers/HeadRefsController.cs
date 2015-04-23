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
    public class HeadRefsController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: HeadRefs
        public ActionResult Index()
        {
            var headRefs = db.HeadRefs.Include(h => h.Head);
            return View(headRefs.ToList());
        }

        // GET: HeadRefs/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            HeadRef headRef = db.HeadRefs.Find(id);
            if (headRef == null)
            {
                return HttpNotFound();
            }
            return View(headRef);
        }

        // GET: HeadRefs/Create
        public ActionResult Create()
        {
            ViewBag.PostId = new SelectList(db.Heads, "PostId", "IAID");
            return View();
        }

        // POST: HeadRefs/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "PostId,HeadRefCount,HeadRefQual,HeadRef1,HeadRefDate")] HeadRef headRef)
        {
            if (ModelState.IsValid)
            {
                db.HeadRefs.Add(headRef);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.PostId = new SelectList(db.Heads, "PostId", "IAID", headRef.PostId);
            return View(headRef);
        }

        // GET: HeadRefs/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            HeadRef headRef = db.HeadRefs.Find(id);
            if (headRef == null)
            {
                return HttpNotFound();
            }
            ViewBag.PostId = new SelectList(db.Heads, "PostId", "IAID", headRef.PostId);
            return View(headRef);
        }

        // POST: HeadRefs/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "PostId,HeadRefCount,HeadRefQual,HeadRef1,HeadRefDate")] HeadRef headRef)
        {
            if (ModelState.IsValid)
            {
                db.Entry(headRef).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.PostId = new SelectList(db.Heads, "PostId", "IAID", headRef.PostId);
            return View(headRef);
        }

        // GET: HeadRefs/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            HeadRef headRef = db.HeadRefs.Find(id);
            if (headRef == null)
            {
                return HttpNotFound();
            }
            return View(headRef);
        }

        // POST: HeadRefs/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            HeadRef headRef = db.HeadRefs.Find(id);
            db.HeadRefs.Remove(headRef);
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
