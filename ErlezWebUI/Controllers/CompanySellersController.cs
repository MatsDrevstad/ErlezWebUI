using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using ErlezWebUI.Models;
using Microsoft.AspNet.Identity;

namespace ErlezWebUI.Controllers
{
    [Authorize]
    public class CompanySellersController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: CompanySellers
        public ActionResult Index()
        {
            var user = User.Identity.GetUserId();
            return View(db.CompanySellers.Where(x => x.ApplicationUser_Id == user).ToList());
        }

        // GET: CompanySellers/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            CompanySeller companySeller = db.CompanySellers.Find(id);
            if (companySeller == null)
            {
                return HttpNotFound();
            }
            return View(companySeller);
        }

        // GET: CompanySellers/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: CompanySellers/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,Name,OrgNo,City")] CompanySeller companySeller)
        {
            if (ModelState.IsValid)
            {
                db.CompanySellers.Add(new CompanySeller
                {
                    Name = companySeller.Name,
                    OrgNo = companySeller.OrgNo,
                    City = companySeller.City,
                    ApplicationUser_Id = User.Identity.GetUserId().ToString(),
                });
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(companySeller);
        }

        // GET: CompanySellers/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            CompanySeller companySeller = db.CompanySellers.Find(id);
            if (companySeller == null)
            {
                return HttpNotFound();
            }
            return View(companySeller);
        }

        // POST: CompanySellers/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,Name,OrgNo,City")] CompanySeller companySeller)
        {
            if (ModelState.IsValid)
            {
                companySeller.ApplicationUser_Id = User.Identity.GetUserId().ToString();
                db.Entry(companySeller).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(companySeller);
        }

        // GET: CompanySellers/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            CompanySeller companySeller = db.CompanySellers.Find(id);
            if (companySeller == null)
            {
                return HttpNotFound();
            }
            return View(companySeller);
        }

        // POST: CompanySellers/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            CompanySeller companySeller = db.CompanySellers.Find(id);
            db.CompanySellers.Remove(companySeller);
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
