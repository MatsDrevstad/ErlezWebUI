using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using ErlezWebUI.Models;
using System.Web.Mvc;
using System.ComponentModel.DataAnnotations;

namespace ErlezWebUI.Models
{
    public class OrdersIndexViewModel
    {
        public IEnumerable<Order> Orders { get; set; }
        public PagingInfo PagingInfo { get; set; }
    }

    public class OrderCreate
    {
        public int Id { get; set; }
        public string OrderType { get; set; }
        public Nullable<System.DateTime> OrderDate { get; set; }
        [Required(ErrorMessage = "Required")]
        [RegularExpression(@"^\$?\d+(\,(\d{1,3}))?$", ErrorMessage = "Siffror med kommatecken för max 3 decimaler.")]
        public string Amount { get; set; }
        public string ArticleName { get; set; }
        public Nullable<System.Guid> Gtin { get; set; }
        public Nullable<int> InvoiceId { get; set; }
        public Nullable<int> CompanySellerId { get; set; }
        public Nullable<int> CompanyBuyerId { get; set; }
        [Required(ErrorMessage = "Required")]
        public string UnitPrice { get; set; }
        [Required(ErrorMessage = "Required")]
        public string UnitType { get; set; }
        public string SelectedValue { get; set; }
        public IEnumerable<System.Web.Mvc.SelectListItem> Articles { get; set; }
    }

    public static class FindAllArticles
    {
        public static IEnumerable<SelectListItem> ToSelectListItems(this IEnumerable<Article> articles, int selectedId)
        {
            return articles.OrderBy(a => a.ArticleName)
                .Select(article => new SelectListItem
                {
                    Selected = (article.Id == selectedId),
                    Text = article.ArticleName,
                    Value = article.Id.ToString()
                });
        }
    }
}