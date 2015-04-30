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
    public class HeadsController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: Heads
        public ActionResult Index()
        {
            var heads = db.Heads.Include(h => h.Bet).Include(h => h.Sum);
            return View(heads.ToList());
        }

        // GET: Heads/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Head head = db.Heads.Find(id);
            if (head == null)
            {
                return HttpNotFound();
            }
            return View(head);
        }

        // GET: Heads/Create
        public ActionResult Create()
        {
            ViewBag.PostId = new SelectList(db.Bets, "PostId", "DueDate");
            ViewBag.PostId = new SelectList(db.Sums, "PostId", "TotalQuantity");
            return View();
        }

        // POST: Heads/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "PostId,IAID,Flow,InvoiceNo,OutputType,UpdateCode,CreateDate,UpdateDate,InvoiceType,InvoiceDate,RealDelDate,RealDelShip,HorizonStartDate,HorizonEndDate,PeriodFrom,PeriodTo,CreditReason,MaterialType,GUID,Timestamp,ErpVersion,isTest,isDeleted,CorporateGroup,Erp,Version,FromID,Hash")] Head head)
        {
            if (ModelState.IsValid)
            {
                db.Heads.Add(head);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.PostId = new SelectList(db.Bets, "PostId", "DueDate", head.PostId);
            ViewBag.PostId = new SelectList(db.Sums, "PostId", "TotalQuantity", head.PostId);
            return View(head);
        }

        // GET: Heads/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Head head = db.Heads.Find(id);
            if (head == null)
            {
                return HttpNotFound();
            }
            ViewBag.PostId = new SelectList(db.Bets, "PostId", "DueDate", head.PostId);
            ViewBag.PostId = new SelectList(db.Sums, "PostId", "TotalQuantity", head.PostId);
            return View(head);
        }

        // POST: Heads/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "PostId,IAID,Flow,InvoiceNo,OutputType,UpdateCode,CreateDate,UpdateDate,InvoiceType,InvoiceDate,RealDelDate,RealDelShip,HorizonStartDate,HorizonEndDate,PeriodFrom,PeriodTo,CreditReason,MaterialType,GUID,Timestamp,ErpVersion,isTest,isDeleted,CorporateGroup,Erp,Version,FromID,Hash")] Head head)
        {
            if (ModelState.IsValid)
            {
                db.Entry(head).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.PostId = new SelectList(db.Bets, "PostId", "DueDate", head.PostId);
            ViewBag.PostId = new SelectList(db.Sums, "PostId", "TotalQuantity", head.PostId);
            return View(head);
        }

        // GET: Heads/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Head head = db.Heads.Find(id);
            if (head == null)
            {
                return HttpNotFound();
            }
            return View(head);
        }

        // POST: Heads/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Head head = db.Heads.Find(id);
            db.Heads.Remove(head);
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
