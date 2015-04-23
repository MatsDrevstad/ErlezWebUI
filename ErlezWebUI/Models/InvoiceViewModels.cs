using ErlezWebUI.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace ErlezWebUI.Models
{
    public class InvoiceEditViewModel
    {
        public IEnumerable<Order> Orders { get; set; }
        public Invoice Invoice { get; set; }

        public string TotalNet { get; set; }
        public string TotalTax { get; set; }
        public string TotalSum { get; set; }
        public string TotalSumRounded { get; set; }
        public string RoundingOff { get; set; }
    }

    public class InvoiceIndexViewModel
    {
        public int Id { get; set; }
        public string SellerName { get; set; }
        public string SellerOrgNo { get; set; }
        public string SellerCity { get; set; }
        public string BuyerName { get; set; }
        public string BuyerOrgNo { get; set; }
        public string BuyerCity { get; set; }
        public Nullable<decimal> TotalNet { get; set; }
        public Nullable<decimal> TotalTax { get; set; }
        public Nullable<decimal> TotalSum { get; set; }
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public Nullable<System.DateTime> InvoiceDate { get; set; }
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public Nullable<System.DateTime> DueDate { get; set; }
        public Nullable<int> InvoiceNo { get; set; }
    }
}