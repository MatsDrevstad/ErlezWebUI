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
    public class OrdersController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();
        public int PageSize = 6;

        // GET: Orders
        public ActionResult Index(int page = 1)
        {
            var orders = db.Orders.Where(o => o.InvoiceId == null)
                .Include(o => o.CompanyBuyer).Include(o => o.CompanySeller).Include(o => o.Invoice);
            OrdersIndexViewModel model = new OrdersIndexViewModel
            {
                Orders = orders.OrderBy(o => o.Id)
                    .Skip((page - 1) * PageSize)
                    .Take(PageSize),
                PagingInfo = new PagingInfo
                {
                    CurrentPage = page,
                    ItemsPerPage = PageSize,
                    TotalItems = orders.Count()
                }
            };
            return View(model);
        }

        // GET: Orders/EvalInvoice/5
        public ActionResult EvalInvoice(int? id, int page = 1)
        {
            if (id == null)
            {
                //return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            int? invoiceNumber;

            //Hämta aktuellt fakturanummer.
            var currentInvoiceNumber = db.Invoices.Max(i => i.InvoiceNo);

            //tilldela värde, första faktura?
            invoiceNumber = currentInvoiceNumber ?? 0;
            invoiceNumber++;

            //kolla om det finns en påbörjad faktura med samma säljare och köpare. (en faktura utan fakturadatum)
            var order = db.Orders.Find(id);
            var compareOrder = db.Orders
                .Where(o => o.CompanyBuyerId == order.CompanyBuyerId)
                .Where(o => o.CompanySellerId == order.CompanySellerId)
                .Where(o => o.InvoiceId.HasValue)
                .FirstOrDefault();

            //tilldela fakturan detta nummer.
            if (compareOrder != null)
            {
                invoiceNumber = compareOrder.InvoiceId;
            }

            //hämta uppgifter om köpare och säljare
            var buyer = db.CompanyBuyers.Find(order.CompanyBuyerId);
            var seller = db.CompanySellers.Find(order.CompanySellerId);

            //skapa fakturan om den inte finns
            if (compareOrder == null)
            {
                db.Invoices.Add(new Invoice()
                {
                    InvoiceNo = invoiceNumber,
                    BuyerName = buyer.Name,
                    BuyerCity = buyer.City,
                    BuyerOrgNo = buyer.OrgNo,
                    SellerName = seller.Name,
                    SellerCity = seller.City,
                    SellerOrgNo = seller.OrgNo,
                });
                db.SaveChanges();
            }

            //koppla till faktura
            db.Orders.Find(order.Id).InvoiceId = db.Invoices.FirstOrDefault(i => i.InvoiceNo == invoiceNumber).Id;
            db.SaveChanges();

            //hämta model för den uppdaterade listan
            var orders = db.Orders.Where(o => o.InvoiceId == null)
                .Include(o => o.CompanyBuyer).Include(o => o.CompanySeller).Include(o => o.Invoice);

            OrdersIndexViewModel model = new OrdersIndexViewModel
            {
                Orders = orders.OrderBy(o => o.Id)
                    .Skip((page - 1) * PageSize)
                    .Take(PageSize),
                PagingInfo = new PagingInfo
                {
                    CurrentPage = page,
                    ItemsPerPage = PageSize,
                    TotalItems = orders.Count()
                }
            };
            return View("Index", model);
        }

        // GET: Orders/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Order order = db.Orders.Find(id);
            if (order == null)
            {
                return HttpNotFound();
            }
            return View(order);
        }

        // GET: Orders/Create
        public ActionResult Create()
        {
            ViewBag.CompanyBuyerId = new SelectList(db.CompanyBuyers, "Id", "Name");
            ViewBag.CompanySellerId = new SelectList(db.CompanySellers, "Id", "Name");
            ViewBag.InvoiceId = new SelectList(db.Invoices, "Id", "InvoiceNo");
            return View();
        }

        [HttpPost]
        public ActionResult Create(int companySellerId, int companyBuyerId)
        {
            var model = new OrderCreate();
            model.CompanyBuyerId = companyBuyerId;
            model.CompanySellerId = companySellerId;
            model.Articles = db.Articles.Where(a => a.CompanySellerId == companySellerId).ToSelectListItems(0);
            return View("CreateOrder", model);
        }

        [HttpPost]
        public ActionResult CreateOrder(OrderCreate vm)
        {
            if (ModelState.IsValid)
            {
                var order = new Order() 
                {
                    OrderType = "280",
                    OrderDate = DateTime.Now,
                    CompanySellerId = vm.CompanySellerId,
                    CompanyBuyerId = vm.CompanyBuyerId,
                    Amount = Convert.ToDecimal(vm.Amount),
                    ArticleName = db.Articles.Find(int.Parse(vm.SelectedValue)).ArticleName,
                    Gtin = Guid.NewGuid(),
                    UnitPrice = Convert.ToDecimal(vm.UnitPrice.Replace('.',',')),
                    UnitType = vm.UnitType,
                };
                db.Orders.Add(order);
                db.SaveChanges();
                return RedirectToAction("Index", "Orders");
            }
            else
            {
                var model = new OrderCreate();
                model.CompanyBuyerId = vm.CompanyBuyerId;
                model.CompanySellerId = vm.CompanySellerId;
                model.Articles = db.Articles.Where(a => a.CompanySellerId == model.CompanySellerId).ToSelectListItems(0);
                return View("CreateOrder", model);
            }
        }

        public JsonResult GetArticles()
        {
            var articles = new List<object>();
            foreach (var item in db.Articles)
            {
                articles.Add(new { 
                    Id = item.Id, 
                    UnitPrice = item.UnitPrice,
                    UnitType = item.UnitType
                });
            }
            return Json(articles, JsonRequestBehavior.AllowGet);
        }

        // GET: Orders/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Order order = db.Orders.Find(id);
            if (order == null)
            {
                return HttpNotFound();
            }
            ViewBag.CompanyBuyerId = new SelectList(db.CompanyBuyers, "Id", "Name", order.CompanyBuyerId);
            ViewBag.CompanySellerId = new SelectList(db.CompanySellers, "Id", "Name", order.CompanySellerId);
            ViewBag.InvoiceId = new SelectList(db.Invoices, "Id", "InvoiceNo", order.InvoiceId);
            return View(order);
        }

        // POST: Orders/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,OrderType,OrderDate,Amount,ArticleName,Gtin,InvoiceId,CompanySellerId,CompanyBuyerId,UnitPrice,UnitType")] Order order)
        {
            if (ModelState.IsValid)
            {
                db.Entry(order).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.CompanyBuyerId = new SelectList(db.CompanyBuyers, "Id", "Name", order.CompanyBuyerId);
            ViewBag.CompanySellerId = new SelectList(db.CompanySellers, "Id", "Name", order.CompanySellerId);
            ViewBag.InvoiceId = new SelectList(db.Invoices, "Id", "InvoiceNo", order.InvoiceId);
            return View(order);
        }

        // GET: Orders/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Order order = db.Orders.Find(id);
            if (order == null)
            {
                return HttpNotFound();
            }
            return View(order);
        }

        // POST: Orders/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Order order = db.Orders.Find(id);
            db.Orders.Remove(order);
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
