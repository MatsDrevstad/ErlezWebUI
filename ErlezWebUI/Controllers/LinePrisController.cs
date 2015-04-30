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
    public class LinePrisController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: LinePris
        public ActionResult Index()
        {
            var linePris = db.LinePris.Include(l => l.Line);
            return View(linePris.ToList());
        }

        // GET: LinePris/Details/5
        public ActionResult Details(long? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            LinePri linePri = db.LinePris.Find(id);
            if (linePri == null)
            {
                return HttpNotFound();
            }
            return View(linePri);
        }

        // GET: LinePris/Create
        public ActionResult Create()
        {
            ViewBag.LineId = new SelectList(db.Lines, "LineId", "LineNumber");
            return View();
        }

        // POST: LinePris/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "LineId,LinePriCount,PriceQualifier,PriceType,PriceTypeQualifier,LinePri1,LinePriBase,LineUnit")] LinePri linePri)
        {
            if (ModelState.IsValid)
            {
                db.LinePris.Add(linePri);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.LineId = new SelectList(db.Lines, "LineId", "LineNumber", linePri.LineId);
            return View(linePri);
        }

        // GET: LinePris/Edit/5
        public ActionResult Edit(long? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            LinePri linePri = db.LinePris.Find(id);
            if (linePri == null)
            {
                return HttpNotFound();
            }
            ViewBag.LineId = new SelectList(db.Lines, "LineId", "LineNumber", linePri.LineId);
            return View(linePri);
        }

        // POST: LinePris/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "LineId,LinePriCount,PriceQualifier,PriceType,PriceTypeQualifier,LinePri1,LinePriBase,LineUnit")] LinePri linePri)
        {
            if (ModelState.IsValid)
            {
                db.Entry(linePri).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.LineId = new SelectList(db.Lines, "LineId", "LineNumber", linePri.LineId);
            return View(linePri);
        }

        // GET: LinePris/Delete/5
        public ActionResult Delete(long? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            LinePri linePri = db.LinePris.Find(id);
            if (linePri == null)
            {
                return HttpNotFound();
            }
            return View(linePri);
        }

        // POST: LinePris/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(long id)
        {
            LinePri linePri = db.LinePris.Find(id);
            db.LinePris.Remove(linePri);
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
