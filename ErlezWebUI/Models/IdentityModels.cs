using System.Data.Entity;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using System;
using System.Collections.Generic;

namespace ErlezWebUI.Models
{
    // You can add profile data for the user by adding more properties to your ApplicationUser class, please visit http://go.microsoft.com/fwlink/?LinkID=317594 to learn more.
    public class ApplicationUser : IdentityUser
    {
        public async Task<ClaimsIdentity> GenerateUserIdentityAsync(UserManager<ApplicationUser> manager)
        {
            // Note the authenticationType must match the one defined in CookieAuthenticationOptions.AuthenticationType
            var userIdentity = await manager.CreateIdentityAsync(this, DefaultAuthenticationTypes.ApplicationCookie);
            // Add custom user claims here
            return userIdentity;
        }

        public int CompanyId { get; set; }
        public string RegisterCompanyName { get; set; }
        public string CompanyName { get; set; }       
    }

    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public ApplicationDbContext()
            : base("DefaultConnection", throwIfV1Schema: false)
        {
        }

        public static ApplicationDbContext Create()
        {
            return new ApplicationDbContext();
        }

        public DbSet<Company> Companies { get; set; }
        public DbSet<Article> Articles { get; set; }
        public DbSet<CompanyBuyer> CompanyBuyers { get; set; }
        public DbSet<CompanySeller> CompanySeller { get; set; }
        public DbSet<Invoice> Invoices { get; set; }
        public DbSet<Order> Orders { get; set; }
    }

    public class Company
    {
        public int Id { get; set; }
        public string CompanyName { get; set; }
    }

    public class Article
    {
        public int Id { get; set; }
        public string ArticleName { get; set; }
        public System.Guid Gtin { get; set; }
        public Nullable<int> CompanySellerId { get; set; }
        public Nullable<decimal> UnitPrice { get; set; }
        public string UnitType { get; set; }

        public virtual CompanySeller CompanySeller { get; set; }
    }

    public class CompanyBuyer
    {
        public CompanyBuyer()
        {
            this.Orders = new HashSet<Order>();
        }

        public int Id { get; set; }
        public string Name { get; set; }
        public string OrgNo { get; set; }
        public string City { get; set; }

        public virtual ICollection<Order> Orders { get; set; }
    }

    public class CompanySeller
    {
        public CompanySeller()
        {
            this.Articles = new HashSet<Article>();
            this.Orders = new HashSet<Order>();
        }

        public int Id { get; set; }
        public string Name { get; set; }
        public string OrgNo { get; set; }
        public string City { get; set; }

        public virtual ICollection<Article> Articles { get; set; }
        public virtual ICollection<Order> Orders { get; set; }
    }

    public class Invoice
    {
        public Invoice()
        {
            this.Orders = new HashSet<Order>();
        }

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
        public Nullable<System.DateTime> InvoiceDate { get; set; }
        public Nullable<System.DateTime> DueDate { get; set; }
        public Nullable<int> InvoiceNo { get; set; }
        public string Status { get; set; }

        public virtual ICollection<Order> Orders { get; set; }
    }

    public class Order
    {
        public int Id { get; set; }
        public string OrderType { get; set; }
        public Nullable<System.DateTime> OrderDate { get; set; }
        public decimal Amount { get; set; }
        public string ArticleName { get; set; }
        public Nullable<System.Guid> Gtin { get; set; }
        public Nullable<int> InvoiceId { get; set; }
        public Nullable<int> CompanySellerId { get; set; }
        public Nullable<int> CompanyBuyerId { get; set; }
        public Nullable<decimal> UnitPrice { get; set; }
        public string UnitType { get; set; }

        public virtual CompanyBuyer CompanyBuyer { get; set; }
        public virtual CompanySeller CompanySeller { get; set; }
        public virtual Invoice Invoice { get; set; }
    }
}