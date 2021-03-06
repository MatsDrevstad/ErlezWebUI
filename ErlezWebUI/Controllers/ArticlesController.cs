﻿using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using ErlezWebUI.Models;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;

namespace ErlezWebUI.Controllers
{
    [Authorize]
    public class ArticlesController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();
        private int PageSize = 6;

        // GET: Articles
        public ActionResult Index(int page = 1)
        {
            var user = User.Identity.GetUserId(); 
            var articles = db.Articles.Where(u => u.ApplicationUser_Id == user).Include(a => a.CompanySeller);
            ArticleIndexViewModel model = new ArticleIndexViewModel
            {
                Articles = articles.OrderBy(o => o.Id)
                    .Skip((page - 1) * PageSize)
                    .Take(PageSize),
                PagingInfo = new PagingInfo
                {
                    CurrentPage = page,
                    ItemsPerPage = PageSize,
                    TotalItems = articles.Count()
                }
            };
            return View(model);
        }

        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var user = User.Identity.GetUserId();
            Article article = db.Articles.Where(u => u.ApplicationUser_Id == user).FirstOrDefault(a => a.Id == id);
            if (article == null)
            {
                return HttpNotFound();
            }
            return View(article);
        }

        // GET: Articles/Create
        public ActionResult Create()
        {
            ViewBag.CompanySellerId = new SelectList(db.CompanySellers, "Id", "Name");
            return View();
        }

        // POST: Articles/Create
        [HttpPost]
        public ActionResult Create(ArticleCreate model)
        {
            if (ModelState.IsValid)
            {
                db.Articles.Add(new Article
                {
                    ArticleName = model.ArticleName,
                    CompanySellerId = model.CompanySellerId,
                    UnitPrice = Convert.ToDecimal(model.UnitPrice),
                    UnitType = model.UnitType,
                    Gtin = Guid.NewGuid(),
                    ApplicationUser_Id = User.Identity.GetUserId().ToString(),
                });
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.CompanySellerId = new SelectList(db.CompanySellers, "Id", "Name", model.CompanySellerId);
            return View(model);
        }

        // GET: Articles/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Article article = db.Articles.Find(id);
            if (article == null)
            {
                return HttpNotFound();
            }
            ViewBag.CompanySellerId = new SelectList(db.CompanySellers, "Id", "Name", article.CompanySellerId);
            ArticleEdit articleEdit = new ArticleEdit()
            {
                Id = article.Id,
                ArticleName = article.ArticleName,
                UnitPrice = article.UnitPrice.ToString(),
                UnitType = article.UnitType
            };
            return View(articleEdit);
        }

        // POST: Articles/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(ArticleEdit article)
        {
            if (ModelState.IsValid)
            {
                var updatedArticle = db.Articles.Find(article.Id);
                updatedArticle.ArticleName = article.ArticleName;
                updatedArticle.UnitPrice = Convert.ToDecimal(article.UnitPrice);
                updatedArticle.UnitType = article.UnitType;
                //db.Entry(article).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.CompanySellerId = new SelectList(db.CompanySellers, "Id", "Name", article.CompanySellerId);
            return View(article);
        }

        // GET: Articles/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Article article = db.Articles.Find(id);
            if (article == null)
            {
                return HttpNotFound();
            }
            return View(article);
        }

        // POST: Articles/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Article article = db.Articles.Find(id);
            db.Articles.Remove(article);
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
