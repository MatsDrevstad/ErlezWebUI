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
    public class AlcsController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: Alcs
        public ActionResult Index()
        {
            var alcs = db.Alcs.Include(a => a.Head);
            return View(alcs.ToList());
        }

        // GET: Alcs/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Alc alc = db.Alcs.Find(id);
            if (alc == null)
            {
                return HttpNotFound();
            }
            return View(alc);
        }

        // GET: Alcs/Create
        public ActionResult Create()
        {
            ViewBag.PostId = new SelectList(db.Heads, "PostId", "IAID");
            return View();
        }

        // POST: Alcs/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "PostId,AlcCount,AlcType,Type,Percentage,BaseAmount,Amount,TaxType,TaxCategory,TaxRate,TaxAmount")] Alc alc)
        {
            if (ModelState.IsValid)
            {
                db.Alcs.Add(alc);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.PostId = new SelectList(db.Heads, "PostId", "IAID", alc.PostId);
            return View(alc);
        }

        // GET: Alcs/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Alc alc = db.Alcs.Find(id);
            if (alc == null)
            {
                return HttpNotFound();
            }
            ViewBag.PostId = new SelectList(db.Heads, "PostId", "IAID", alc.PostId);
            return View(alc);
        }

        // POST: Alcs/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "PostId,AlcCount,AlcType,Type,Percentage,BaseAmount,Amount,TaxType,TaxCategory,TaxRate,TaxAmount")] Alc alc)
        {
            if (ModelState.IsValid)
            {
                db.Entry(alc).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.PostId = new SelectList(db.Heads, "PostId", "IAID", alc.PostId);
            return View(alc);
        }

        // GET: Alcs/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Alc alc = db.Alcs.Find(id);
            if (alc == null)
            {
                return HttpNotFound();
            }
            return View(alc);
        }

        // POST: Alcs/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Alc alc = db.Alcs.Find(id);
            db.Alcs.Remove(alc);
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
