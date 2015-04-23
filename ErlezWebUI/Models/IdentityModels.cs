using System.Data.Entity;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration.Conventions;

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

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            //code first migrations: decimal precision and scale
            modelBuilder.Entity<Article>().Property(a => a.UnitPrice).HasPrecision(15, 4);
            modelBuilder.Entity<Invoice>().Property(i => i.TotalNet).HasPrecision(15, 4);
            modelBuilder.Entity<Invoice>().Property(i => i.TotalSum).HasPrecision(15, 4);
            modelBuilder.Entity<Invoice>().Property(i => i.TotalTax).HasPrecision(15, 4);

            base.OnModelCreating(modelBuilder);

            //kör pluralize på users-tabellerna
            base.OnModelCreating(modelBuilder);

            //ta bort på övriga
            modelBuilder.Conventions.Remove<PluralizingTableNameConvention>();
        }

        //Business domain
        public DbSet<CompanyB2> CompanyB2s { get; set; }
        public DbSet<Article> Articles { get; set; }
        public DbSet<CompanyBuyer> CompanyBuyers { get; set; }
        public DbSet<CompanySeller> CompanySellers { get; set; }
        public DbSet<Invoice> Invoices { get; set; }
        public DbSet<Order> Orders { get; set; }
        //Gross domain
        public DbSet<Alc> Alcs { get; set; }
        public DbSet<Bet> Bets { get; set; }
        public DbSet<Company> Companies { get; set; }
        public DbSet<Enclosure> Enclosures { get; set; }
        public DbSet<Head> Heads { get; set; }
        public DbSet<HeadRef> HeadRefs { get; set; }
        public DbSet<Line> Lines { get; set; }
        public DbSet<LineAlc> LineAlcs { get; set; }
        public DbSet<LinePri> LinePris { get; set; }
        public DbSet<LineRef> LineRefs { get; set; }
        public DbSet<LineTax> LineTaxes { get; set; }
        public DbSet<Sum> Sums { get; set; }
        public DbSet<SumTax> SumTaxes { get; set; }
        public DbSet<Tdt> Tdts { get; set; }
        public DbSet<Tod> Tods { get; set; }
    }

    public class CompanyB2
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

    public class Alc
    {
        [Key]
        public int PostId { get; set; }
        public int AlcCount { get; set; }
        public string AlcType { get; set; }
        public string Type { get; set; }
        public string Percentage { get; set; }
        public string BaseAmount { get; set; }
        public string Amount { get; set; }
        public string TaxType { get; set; }
        public string TaxCategory { get; set; }
        public string TaxRate { get; set; }
        public string TaxAmount { get; set; }

        public virtual Head Head { get; set; }
    }

    public class Bet
    {
        [Key]
        [ForeignKey("Head")]
        public int PostId { get; set; }
        public string DueDate { get; set; }
        public string DueDays { get; set; }
        public string InvCurrency { get; set; }
        public string TaxCurrency { get; set; }
        public string TaxCurrencyRate { get; set; }
        public string TaxCurrencyRateDate { get; set; }
        public string ExemptFromTax { get; set; }
        public string PenaltySurchargePercent { get; set; }
        public string PaymentInstruction { get; set; }

        public virtual Head Head { get; set; }
    }

    public class Company
    {
        [Key]
        public int PostId { get; set; }
        public int CompanyCount { get; set; }
        public string CompanyQual { get; set; }
        public string PartyIdentification { get; set; }
        public string VatNo { get; set; }
        public string OrgNo { get; set; }
        public string CustomerNumber { get; set; }
        public string SellerUniqueIdentification { get; set; }
        public string Name { get; set; }
        public string ContactName { get; set; }
        public string ContactTel { get; set; }
        public string ContactEmail { get; set; }
        public string ContactFax { get; set; }
        public string StreetName { get; set; }
        public string AdditionalStreetName { get; set; }
        public string ZipCode { get; set; }
        public string City { get; set; }
        public string Country { get; set; }
        public string Box { get; set; }
        public string Department { get; set; }
        public string BankCode { get; set; }
        public string BankName { get; set; }
        public string BankAccount { get; set; }
        public string Iban { get; set; }
        public string PlusGiro { get; set; }
        public string BankGiro { get; set; }

        public virtual Head Head { get; set; }
    }

    public class Enclosure
    {
        [Key]
        public int PostId { get; set; }
        public int EnclosureCount { get; set; }
        public string MediaType { get; set; }
        public string FileName { get; set; }
        public string FileCreationDate { get; set; }
        public string EnclosedDataFormat { get; set; }
        public string EnclosedData { get; set; }

        public virtual Head Head { get; set; }
    }

    public class Head
    {
        public Head()
        {
            this.Alcs = new HashSet<Alc>();
            this.Companies = new HashSet<Company>();
            this.Enclosures = new HashSet<Enclosure>();
            this.HeadRefs = new HashSet<HeadRef>();
            this.Lines = new HashSet<Line>();
            this.SumTaxes = new HashSet<SumTax>();
            this.Tdts = new HashSet<Tdt>();
            this.Tods = new HashSet<Tod>();
        }

        [Key]
        public int PostId { get; set; }
        public string IAID { get; set; }
        public string Flow { get; set; }
        public string InvoiceNo { get; set; }
        public string OutputType { get; set; }
        public Nullable<int> UpdateCode { get; set; }
        public Nullable<System.DateTime> CreateDate { get; set; }
        public Nullable<System.DateTime> UpdateDate { get; set; }
        public string InvoiceType { get; set; }
        public string InvoiceDate { get; set; }
        public string RealDelDate { get; set; }
        public string RealDelShip { get; set; }
        public string HorizonStartDate { get; set; }
        public string HorizonEndDate { get; set; }
        public string PeriodFrom { get; set; }
        public string PeriodTo { get; set; }
        public string CreditReason { get; set; }
        public string MaterialType { get; set; }
        public string GUID { get; set; }
        public string Timestamp { get; set; }
        public string ErpVersion { get; set; }
        public string isTest { get; set; }
        public Nullable<bool> isDeleted { get; set; }
        public string CorporateGroup { get; set; }
        public string Erp { get; set; }
        public string Version { get; set; }
        public Nullable<int> FromID { get; set; }
        public string Hash { get; set; }

        public virtual ICollection<Alc> Alcs { get; set; }
        public virtual Bet Bet { get; set; }
        public virtual ICollection<Company> Companies { get; set; }
        public virtual ICollection<Enclosure> Enclosures { get; set; }
        public virtual ICollection<HeadRef> HeadRefs { get; set; }
        public virtual ICollection<Line> Lines { get; set; }
        public virtual Sum Sum { get; set; }
        public virtual ICollection<SumTax> SumTaxes { get; set; }
        public virtual ICollection<Tdt> Tdts { get; set; }
        public virtual ICollection<Tod> Tods { get; set; }
    }

    public class HeadRef
    {
        [Key]
        public int PostId { get; set; }
        public int HeadRefCount { get; set; }
        public string HeadRefQual { get; set; }
        public string HeadRef1 { get; set; }
        public string HeadRefDate { get; set; }

        public virtual Head Head { get; set; }
    }

    public class Line
    {
        public Line()
        {
            this.LineAlcs = new HashSet<LineAlc>();
            this.LinePris = new HashSet<LinePri>();
            this.LineRefs = new HashSet<LineRef>();
            this.LineTaxes = new HashSet<LineTax>();
        }

        [Key]
        public int PostId { get; set; }
        public long LineId { get; set; }
        public int LineCount { get; set; }
        public string LineNumber { get; set; }
        public string GtinNo { get; set; }
        public string SellerPartNo { get; set; }
        public string BuyerPartNo { get; set; }
        public string ManufPartNo { get; set; }
        public string QuantityInvoiced { get; set; }
        public string QuantityInvoicedUnit { get; set; }
        public string QuantityDelivered { get; set; }
        public string QuantityDeliveredUnit { get; set; }
        public string QuantityDespatched { get; set; }
        public string QuantityDespatchedUnit { get; set; }
        public string QuantityReceived { get; set; }
        public string QuantityReceivedUnit { get; set; }
        public string LineAmount { get; set; }
        public string Description { get; set; }
        public string Description2 { get; set; }
        public string CreditReason { get; set; }
        public string Revision { get; set; }
        public string Origin { get; set; }
        public string DelDate { get; set; }
        public string FirstDelDate { get; set; }
        public string LastDelDate { get; set; }
        public string LineType { get; set; }

        public virtual Head Head { get; set; }
        public virtual ICollection<LineAlc> LineAlcs { get; set; }
        public virtual ICollection<LinePri> LinePris { get; set; }
        public virtual ICollection<LineRef> LineRefs { get; set; }
        public virtual ICollection<LineTax> LineTaxes { get; set; }
    }

    public class LineAlc
    {
        [Key]
        public long LineId { get; set; }
        public int LineAlcCount { get; set; }
        public string AlcType { get; set; }
        public string Type { get; set; }
        public string Percentage { get; set; }
        public string Amount { get; set; }
        public string TaxType { get; set; }
        public string TaxCategory { get; set; }
        public string TaxRate { get; set; }
        public string TaxAmount { get; set; }
        public string BaseAmount { get; set; }

        public virtual Line Line { get; set; }
    }

    public class LinePri
    {
        [Key]
        public long LineId { get; set; }
        public int LinePriCount { get; set; }
        public string PriceQualifier { get; set; }
        public string PriceType { get; set; }
        public string PriceTypeQualifier { get; set; }
        public string LinePri1 { get; set; }
        public string LinePriBase { get; set; }
        public string LineUnit { get; set; }

        public virtual Line Line { get; set; }
    }

    public class LineRef
    {
        [Key]
        public long LineId { get; set; }
        public int LineRefCount { get; set; }
        public string LineRefQual { get; set; }
        public string LineRef1 { get; set; }
        public string LineRefLin { get; set; }
        public string LineRefDate { get; set; }

        public virtual Line Line { get; set; }
    }

    public class LineTax
    {
        [Key]
        public long LineId { get; set; }
        public int LineTaxCount { get; set; }
        public string LineTaxType { get; set; }
        public string LineTaxCategory { get; set; }
        public string LineTaxRate { get; set; }
        public string LineTaxAmount { get; set; }
        public string ExemptFromTax { get; set; }

        public virtual Line Line { get; set; }
    }

    public class Sum
    {
        [Key]
        [ForeignKey("Head")]
        public int PostId { get; set; }
        public string TotalQuantity { get; set; }
        public string TotalLines { get; set; }
        public string TotalAmount { get; set; }
        public string TotalAmountTaxCur { get; set; }
        public string LineAmount { get; set; }
        public string TaxableAmount { get; set; }
        public string TaxableAmountTaxCur { get; set; }
        public string TaxableAmountExclExemption { get; set; }
        public string TaxableAmountExclExemptionTaxCur { get; set; }
        public string TaxAmount { get; set; }
        public string TaxAmountTaxCur { get; set; }
        public string AlcAmount { get; set; }
        public string AdjustmentAmount { get; set; }
        public string NonTaxableAmount { get; set; }
        public string ExemptionAmount { get; set; }

        public virtual Head Head { get; set; }
    }

    public class SumTax
    {
        [Key]
        public int PostId { get; set; }
        public int SumTaxCount { get; set; }
        public string SumTaxType { get; set; }
        public string SumTaxCategory { get; set; }
        public string SumTaxRate { get; set; }
        public string SumTaxableAmount { get; set; }
        public string SumTaxAmount { get; set; }
        public string TaxCurrencyTaxAmount { get; set; }

        public virtual Head Head { get; set; }
    }

    public class Tdt
    {
        [Key]
        public int PostId { get; set; }
        public int TdtCount { get; set; }
        public string TdtMethod { get; set; }
        public string TdtDescription { get; set; }
        public string TdtCarrierName { get; set; }

        public virtual Head Head { get; set; }
    }

    public class Tod
    {
        [Key]
        public int PostId { get; set; }
        public int TodCount { get; set; }
        public string TodTerms { get; set; }
        public string TodDescription { get; set; }
        public string TodTermsLoc { get; set; }

        public virtual Head Head { get; set; }
    }
}