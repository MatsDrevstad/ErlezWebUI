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
    public class LineTaxesController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: LineTaxes
        public ActionResult Index()
        {
            var lineTaxes = db.LineTaxes.Include(l => l.Line);
            return View(lineTaxes.ToList());
        }

        // GET: LineTaxes/Details/5
        public ActionResult Details(long? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            LineTax lineTax = db.LineTaxes.Find(id);
            if (lineTax == null)
            {
                return HttpNotFound();
            }
            return View(lineTax);
        }

        // GET: LineTaxes/Create
        public ActionResult Create()
        {
            ViewBag.LineId = new SelectList(db.Lines, "LineId", "LineNumber");
            return View();
        }

        // POST: LineTaxes/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "LineId,LineTaxCount,LineTaxType,LineTaxCategory,LineTaxRate,LineTaxAmount,ExemptFromTax")] LineTax lineTax)
        {
            if (ModelState.IsValid)
            {
                db.LineTaxes.Add(lineTax);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.LineId = new SelectList(db.Lines, "LineId", "LineNumber", lineTax.LineId);
            return View(lineTax);
        }

        // GET: LineTaxes/Edit/5
        public ActionResult Edit(long? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            LineTax lineTax = db.LineTaxes.Find(id);
            if (lineTax == null)
            {
                return HttpNotFound();
            }
            ViewBag.LineId = new SelectList(db.Lines, "LineId", "LineNumber", lineTax.LineId);
            return View(lineTax);
        }

        // POST: LineTaxes/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "LineId,LineTaxCount,LineTaxType,LineTaxCategory,LineTaxRate,LineTaxAmount,ExemptFromTax")] LineTax lineTax)
        {
            if (ModelState.IsValid)
            {
                db.Entry(lineTax).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.LineId = new SelectList(db.Lines, "LineId", "LineNumber", lineTax.LineId);
            return View(lineTax);
        }

        // GET: LineTaxes/Delete/5
        public ActionResult Delete(long? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            LineTax lineTax = db.LineTaxes.Find(id);
            if (lineTax == null)
            {
                return HttpNotFound();
            }
            return View(lineTax);
        }

        // POST: LineTaxes/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(long id)
        {
            LineTax lineTax = db.LineTaxes.Find(id);
            db.LineTaxes.Remove(lineTax);
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
