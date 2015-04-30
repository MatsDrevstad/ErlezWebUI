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
    public class TodsController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: Tods
        public ActionResult Index()
        {
            var tods = db.Tods.Include(t => t.Head);
            return View(tods.ToList());
        }

        // GET: Tods/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Tod tod = db.Tods.Find(id);
            if (tod == null)
            {
                return HttpNotFound();
            }
            return View(tod);
        }

        // GET: Tods/Create
        public ActionResult Create()
        {
            ViewBag.PostId = new SelectList(db.Heads, "PostId", "IAID");
            return View();
        }

        // POST: Tods/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "PostId,TodCount,TodTerms,TodDescription,TodTermsLoc")] Tod tod)
        {
            if (ModelState.IsValid)
            {
                db.Tods.Add(tod);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.PostId = new SelectList(db.Heads, "PostId", "IAID", tod.PostId);
            return View(tod);
        }

        // GET: Tods/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Tod tod = db.Tods.Find(id);
            if (tod == null)
            {
                return HttpNotFound();
            }
            ViewBag.PostId = new SelectList(db.Heads, "PostId", "IAID", tod.PostId);
            return View(tod);
        }

        // POST: Tods/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "PostId,TodCount,TodTerms,TodDescription,TodTermsLoc")] Tod tod)
        {
            if (ModelState.IsValid)
            {
                db.Entry(tod).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.PostId = new SelectList(db.Heads, "PostId", "IAID", tod.PostId);
            return View(tod);
        }

        // GET: Tods/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Tod tod = db.Tods.Find(id);
            if (tod == null)
            {
                return HttpNotFound();
            }
            return View(tod);
        }

        // POST: Tods/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Tod tod = db.Tods.Find(id);
            db.Tods.Remove(tod);
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
