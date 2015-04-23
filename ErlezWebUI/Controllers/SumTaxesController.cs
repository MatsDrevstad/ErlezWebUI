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
    public class SumTaxesController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: SumTaxes
        public ActionResult Index()
        {
            var sumTaxes = db.SumTaxes.Include(s => s.Head);
            return View(sumTaxes.ToList());
        }

        // GET: SumTaxes/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            SumTax sumTax = db.SumTaxes.Find(id);
            if (sumTax == null)
            {
                return HttpNotFound();
            }
            return View(sumTax);
        }

        // GET: SumTaxes/Create
        public ActionResult Create()
        {
            ViewBag.PostId = new SelectList(db.Heads, "PostId", "IAID");
            return View();
        }

        // POST: SumTaxes/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "PostId,SumTaxCount,SumTaxType,SumTaxCategory,SumTaxRate,SumTaxableAmount,SumTaxAmount,TaxCurrencyTaxAmount")] SumTax sumTax)
        {
            if (ModelState.IsValid)
            {
                db.SumTaxes.Add(sumTax);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.PostId = new SelectList(db.Heads, "PostId", "IAID", sumTax.PostId);
            return View(sumTax);
        }

        // GET: SumTaxes/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            SumTax sumTax = db.SumTaxes.Find(id);
            if (sumTax == null)
            {
                return HttpNotFound();
            }
            ViewBag.PostId = new SelectList(db.Heads, "PostId", "IAID", sumTax.PostId);
            return View(sumTax);
        }

        // POST: SumTaxes/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "PostId,SumTaxCount,SumTaxType,SumTaxCategory,SumTaxRate,SumTaxableAmount,SumTaxAmount,TaxCurrencyTaxAmount")] SumTax sumTax)
        {
            if (ModelState.IsValid)
            {
                db.Entry(sumTax).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.PostId = new SelectList(db.Heads, "PostId", "IAID", sumTax.PostId);
            return View(sumTax);
        }

        // GET: SumTaxes/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            SumTax sumTax = db.SumTaxes.Find(id);
            if (sumTax == null)
            {
                return HttpNotFound();
            }
            return View(sumTax);
        }

        // POST: SumTaxes/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            SumTax sumTax = db.SumTaxes.Find(id);
            db.SumTaxes.Remove(sumTax);
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
