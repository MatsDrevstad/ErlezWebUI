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
    public class LineAlcsController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: LineAlcs
        public ActionResult Index()
        {
            var lineAlcs = db.LineAlcs.Include(l => l.Line);
            return View(lineAlcs.ToList());
        }

        // GET: LineAlcs/Details/5
        public ActionResult Details(long? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            LineAlc lineAlc = db.LineAlcs.Find(id);
            if (lineAlc == null)
            {
                return HttpNotFound();
            }
            return View(lineAlc);
        }

        // GET: LineAlcs/Create
        public ActionResult Create()
        {
            ViewBag.LineId = new SelectList(db.Lines, "LineId", "LineNumber");
            return View();
        }

        // POST: LineAlcs/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "LineId,LineAlcCount,AlcType,Type,Percentage,Amount,TaxType,TaxCategory,TaxRate,TaxAmount,BaseAmount")] LineAlc lineAlc)
        {
            if (ModelState.IsValid)
            {
                db.LineAlcs.Add(lineAlc);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.LineId = new SelectList(db.Lines, "LineId", "LineNumber", lineAlc.LineId);
            return View(lineAlc);
        }

        // GET: LineAlcs/Edit/5
        public ActionResult Edit(long? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            LineAlc lineAlc = db.LineAlcs.Find(id);
            if (lineAlc == null)
            {
                return HttpNotFound();
            }
            ViewBag.LineId = new SelectList(db.Lines, "LineId", "LineNumber", lineAlc.LineId);
            return View(lineAlc);
        }

        // POST: LineAlcs/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "LineId,LineAlcCount,AlcType,Type,Percentage,Amount,TaxType,TaxCategory,TaxRate,TaxAmount,BaseAmount")] LineAlc lineAlc)
        {
            if (ModelState.IsValid)
            {
                db.Entry(lineAlc).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.LineId = new SelectList(db.Lines, "LineId", "LineNumber", lineAlc.LineId);
            return View(lineAlc);
        }

        // GET: LineAlcs/Delete/5
        public ActionResult Delete(long? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            LineAlc lineAlc = db.LineAlcs.Find(id);
            if (lineAlc == null)
            {
                return HttpNotFound();
            }
            return View(lineAlc);
        }

        // POST: LineAlcs/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(long id)
        {
            LineAlc lineAlc = db.LineAlcs.Find(id);
            db.LineAlcs.Remove(lineAlc);
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
