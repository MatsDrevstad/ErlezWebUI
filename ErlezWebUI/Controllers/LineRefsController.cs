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
    public class LineRefsController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: LineRefs
        public ActionResult Index()
        {
            var lineRefs = db.LineRefs.Include(l => l.Line);
            return View(lineRefs.ToList());
        }

        // GET: LineRefs/Details/5
        public ActionResult Details(long? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            LineRef lineRef = db.LineRefs.Find(id);
            if (lineRef == null)
            {
                return HttpNotFound();
            }
            return View(lineRef);
        }

        // GET: LineRefs/Create
        public ActionResult Create()
        {
            ViewBag.LineId = new SelectList(db.Lines, "LineId", "LineNumber");
            return View();
        }

        // POST: LineRefs/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "LineId,LineRefCount,LineRefQual,LineRef1,LineRefLin,LineRefDate")] LineRef lineRef)
        {
            if (ModelState.IsValid)
            {
                db.LineRefs.Add(lineRef);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.LineId = new SelectList(db.Lines, "LineId", "LineNumber", lineRef.LineId);
            return View(lineRef);
        }

        // GET: LineRefs/Edit/5
        public ActionResult Edit(long? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            LineRef lineRef = db.LineRefs.Find(id);
            if (lineRef == null)
            {
                return HttpNotFound();
            }
            ViewBag.LineId = new SelectList(db.Lines, "LineId", "LineNumber", lineRef.LineId);
            return View(lineRef);
        }

        // POST: LineRefs/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "LineId,LineRefCount,LineRefQual,LineRef1,LineRefLin,LineRefDate")] LineRef lineRef)
        {
            if (ModelState.IsValid)
            {
                db.Entry(lineRef).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.LineId = new SelectList(db.Lines, "LineId", "LineNumber", lineRef.LineId);
            return View(lineRef);
        }

        // GET: LineRefs/Delete/5
        public ActionResult Delete(long? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            LineRef lineRef = db.LineRefs.Find(id);
            if (lineRef == null)
            {
                return HttpNotFound();
            }
            return View(lineRef);
        }

        // POST: LineRefs/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(long id)
        {
            LineRef lineRef = db.LineRefs.Find(id);
            db.LineRefs.Remove(lineRef);
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
