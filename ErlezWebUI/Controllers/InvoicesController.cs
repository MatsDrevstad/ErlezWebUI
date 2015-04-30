using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using ErlezWebUI.Models;
using System.Collections;

namespace ErlezWebUI.Controllers
{
    [Authorize]
    public class InvoicesController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: Invoices
        public ActionResult Index()
        {
            var invoices = new List<InvoiceIndexViewModel>();
            foreach (var item in db.Invoices)
            {
                invoices.Add(new InvoiceIndexViewModel
                {
                    Id = item.Id,
                    SellerName = item.SellerName,
                    SellerOrgNo = item.SellerOrgNo,
                    SellerCity = item.SellerCity,
                    BuyerName = item.BuyerName,
                    BuyerOrgNo = item.BuyerOrgNo,
                    BuyerCity = item.BuyerCity,
                    TotalNet = item.TotalNet,
                    TotalTax = item.TotalTax,
                    TotalSum = item.TotalSum,
                    DueDate = item.DueDate,
                    InvoiceDate = item.InvoiceDate,
                    InvoiceNo = item.InvoiceNo,
                });
            }

            var invoicesGrouped = invoices
                .GroupBy(i => i.InvoiceNo)
                .Select(grp => grp.First())
                .ToList();

            return View(invoicesGrouped);
        }

        // GET: Invoices/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Invoice invoice = db.Invoices.Find(id);
            if (invoice == null)
            {
                return HttpNotFound();
            }
            return View(invoice);
        }

        // GET: Invoices/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Invoices/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,InvoiceNo,SellerName,SellerOrgNo,SellerCity,BuyerName,BuyerOrgNo,BuyerCity,TotalNet,TotalTax,TotalSum,InvoiceDate,DueDate")] Invoice invoice)
        {
            if (ModelState.IsValid)
            {
                db.Invoices.Add(invoice);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(invoice);
        }

        // GET: Invoices/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Invoice invoice = db.Invoices.Find(id);
            if (invoice == null)
            {
                return HttpNotFound();
            }
            var model = new InvoiceEditViewModel();

            Nullable<decimal> TotalNet = 0;
            Nullable<decimal> TotalTax = 0;
            Nullable<decimal> TotalSum = 0;
            var RoundingOff = 0m;
            var TotalSumRounded = 0m;

            foreach (var item in invoice.Orders)
            {
                TotalNet = TotalNet + item.UnitPrice * item.Amount;
            }
            TotalTax = TotalNet * 0.25m;
            TotalSum = TotalNet + TotalTax;
            RoundingOff = Convert.ToDecimal(TotalSum) - Math.Round(Convert.ToDecimal(TotalSum));
            TotalSumRounded = (RoundingOff > 0) ? Convert.ToDecimal(TotalSum) - RoundingOff : Convert.ToDecimal(TotalSum) + (RoundingOff * -1);

            model.TotalNet = Convert.ToDecimal(TotalNet).ToString("C");
            model.TotalTax = Convert.ToDecimal(TotalTax).ToString("C");
            model.TotalSum = Math.Round(Convert.ToDecimal(TotalSum), 2).ToString("C");
            string FormatCharacter = (RoundingOff > 0) ? "" : "+";
            model.RoundingOff = FormatCharacter + (RoundingOff * -1).ToString("C");
            model.TotalSumRounded = Convert.ToDecimal(TotalSumRounded).ToString("C");

            model.Invoice = invoice;
            model.Invoice.TotalNet = TotalNet;
            model.Invoice.TotalTax = TotalTax;
            model.Invoice.TotalSum = TotalSumRounded;
            if (model.Invoice.InvoiceDate == null)
            {
                model.Invoice.InvoiceDate = DateTime.Now;
                model.Invoice.DueDate = DateTime.Now.AddDays(30);
            }
            model.Orders = db.Orders.Where(o => o.InvoiceId == invoice.Id);
            return View(model);
        }

        // POST: Invoices/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,InvoiceNo,SellerName,SellerOrgNo,SellerCity,BuyerName,BuyerOrgNo,BuyerCity,TotalNet,TotalTax,TotalSum,InvoiceDate,DueDate")] Invoice invoice)
        {
            if (ModelState.IsValid)
            {
                db.Entry(invoice).State = EntityState.Modified;
                invoice.Status = "Created";
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(invoice);
        }

        // GET: Invoices/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Invoice invoice = db.Invoices.Find(id);
            if (invoice == null)
            {
                return HttpNotFound();
            }
            return View(invoice);
        }

        // POST: Invoices/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Invoice invoice = db.Invoices.Find(id);
            db.Invoices.Remove(invoice);
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
