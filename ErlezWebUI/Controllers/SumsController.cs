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
    public class SumsController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: Sums
        public ActionResult Index()
        {
            var sums = db.Sums.Include(s => s.Head);
            return View(sums.ToList());
        }

        // GET: Sums/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Sum sum = db.Sums.Find(id);
            if (sum == null)
            {
                return HttpNotFound();
            }
            return View(sum);
        }

        // GET: Sums/Create
        public ActionResult Create()
        {
            ViewBag.PostId = new SelectList(db.Heads, "PostId", "IAID");
            return View();
        }

        // POST: Sums/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "PostId,TotalQuantity,TotalLines,TotalAmount,TotalAmountTaxCur,LineAmount,TaxableAmount,TaxableAmountTaxCur,TaxableAmountExclExemption,TaxableAmountExclExemptionTaxCur,TaxAmount,TaxAmountTaxCur,AlcAmount,AdjustmentAmount,NonTaxableAmount,ExemptionAmount")] Sum sum)
        {
            if (ModelState.IsValid)
            {
                db.Sums.Add(sum);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.PostId = new SelectList(db.Heads, "PostId", "IAID", sum.PostId);
            return View(sum);
        }

        // GET: Sums/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Sum sum = db.Sums.Find(id);
            if (sum == null)
            {
                return HttpNotFound();
            }
            ViewBag.PostId = new SelectList(db.Heads, "PostId", "IAID", sum.PostId);
            return View(sum);
        }

        // POST: Sums/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "PostId,TotalQuantity,TotalLines,TotalAmount,TotalAmountTaxCur,LineAmount,TaxableAmount,TaxableAmountTaxCur,TaxableAmountExclExemption,TaxableAmountExclExemptionTaxCur,TaxAmount,TaxAmountTaxCur,AlcAmount,AdjustmentAmount,NonTaxableAmount,ExemptionAmount")] Sum sum)
        {
            if (ModelState.IsValid)
            {
                db.Entry(sum).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.PostId = new SelectList(db.Heads, "PostId", "IAID", sum.PostId);
            return View(sum);
        }

        // GET: Sums/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Sum sum = db.Sums.Find(id);
            if (sum == null)
            {
                return HttpNotFound();
            }
            return View(sum);
        }

        // POST: Sums/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Sum sum = db.Sums.Find(id);
            db.Sums.Remove(sum);
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
