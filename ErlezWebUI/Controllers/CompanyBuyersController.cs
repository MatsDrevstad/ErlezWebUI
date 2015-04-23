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
    public class CompanyBuyersController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: CompanyBuyers
        public ActionResult Index()
        {
            return View(db.CompanyBuyers.ToList());
        }

        // GET: CompanyBuyers/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            CompanyBuyer companyBuyer = db.CompanyBuyers.Find(id);
            if (companyBuyer == null)
            {
                return HttpNotFound();
            }
            return View(companyBuyer);
        }

        // GET: CompanyBuyers/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: CompanyBuyers/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,Name,OrgNo,City")] CompanyBuyer companyBuyer)
        {
            if (ModelState.IsValid)
            {
                db.CompanyBuyers.Add(companyBuyer);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(companyBuyer);
        }

        // GET: CompanyBuyers/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            CompanyBuyer companyBuyer = db.CompanyBuyers.Find(id);
            if (companyBuyer == null)
            {
                return HttpNotFound();
            }
            return View(companyBuyer);
        }

        // POST: CompanyBuyers/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,Name,OrgNo,City")] CompanyBuyer companyBuyer)
        {
            if (ModelState.IsValid)
            {
                db.Entry(companyBuyer).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(companyBuyer);
        }

        // GET: CompanyBuyers/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            CompanyBuyer companyBuyer = db.CompanyBuyers.Find(id);
            if (companyBuyer == null)
            {
                return HttpNotFound();
            }
            return View(companyBuyer);
        }

        // POST: CompanyBuyers/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            CompanyBuyer companyBuyer = db.CompanyBuyers.Find(id);
            db.CompanyBuyers.Remove(companyBuyer);
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
