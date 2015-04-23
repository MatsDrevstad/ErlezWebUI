using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Linq;
using System.Reflection;
using System.Web;
using System.Web.Mvc;

namespace ErlezWebUI.Controllers
{
    public class ResetDbController : Controller
    {
        // GET: ResetDb
        [HandleError]
        public ActionResult Index()
        {
            try
            {
                DbGenerate.ResetDb();
                return View();
            }
            catch (Exception ex)
            {
                ViewBag.ErrorMessage = ex.Message;
                return View("Error");
            }
        }
    }

    class DbGenerate
    {
        public static void ResetDb()
        {
            var db = new DbHelper();

            try
            {
                SqlCommand command1 = new SqlCommand(DbDesign.dropDb, db.GetConnection());
                SqlCommand command2 = new SqlCommand(DbDesign.createDb, db.GetConnection());
                SqlCommand command3 = new SqlCommand(DbDesign.alterDb, db.GetConnection());
                SqlCommand command4 = new SqlCommand(DbDesign.insertDb, db.GetConnection());
                db.Open();
                command1.ExecuteNonQuery();
                command2.ExecuteNonQuery();
                command3.ExecuteNonQuery();
                command4.ExecuteNonQuery();
            }
            catch (Exception e)
            {
                throw e;
            }
            finally
            {
                db.Close();
            }
        }
    }

    public class DbDesign
    {
        //Bull

        public static string dropDb = @"DROP TABLE [dbo].[Article];
                                        DROP TABLE [dbo].[Order];
                                        DROP TABLE [dbo].[CompanySeller];
                                        DROP TABLE [dbo].[CompanyBuyer];
                                        DROP TABLE [dbo].[Invoice]";

        public static string createDb = @"CREATE TABLE [dbo].[Article](
	            [Id] [int] IDENTITY(1,1) NOT NULL,
	            [ArticleName] [varchar](50) NOT NULL,
	            [Gtin] [uniqueidentifier] NOT NULL CONSTRAINT [DF_Article_Gtin]  DEFAULT (newid()),
	            [CompanySellerId] [int] NULL,
	            [UnitPrice] [decimal](15, 4) NULL,
	            [UnitType] [varchar](50) NULL,
                CONSTRAINT [PK_Article] PRIMARY KEY CLUSTERED ([Id] ASC));
            CREATE TABLE [dbo].[CompanyBuyer](
	            [Id] [int] IDENTITY(1,1) NOT NULL,
	            [Name] [varchar](50) NOT NULL,
	            [OrgNo] [varchar](50) NULL,
	            [City] [varchar](50) NULL,
                CONSTRAINT [PK_CompanyBuyer] PRIMARY KEY CLUSTERED ([Id] ASC));
            CREATE TABLE [dbo].[CompanySeller](
	            [Id] [int] IDENTITY(1,1) NOT NULL,
	            [Name] [varchar](50) NOT NULL,
	            [OrgNo] [varchar](50) NULL,
	            [City] [varchar](50) NULL,
                CONSTRAINT [PK_CompanySeller] PRIMARY KEY CLUSTERED ([Id] ASC));
            CREATE TABLE [dbo].[Invoice](
	            [Id] [int] IDENTITY(1,1) NOT NULL,
	            [InvoiceNo] [int] NULL,
	            [SellerName] [varchar](50) NULL,
	            [SellerOrgNo] [varchar](50) NULL,
	            [SellerCity] [varchar](50) NULL,
	            [BuyerName] [varchar](50) NULL,
	            [BuyerOrgNo] [varchar](50) NULL,
	            [BuyerCity] [varchar](50) NULL,
	            [Status] [varchar](50) NULL,
	            [TotalNet] [decimal](15, 4) NULL,
	            [TotalTax] [decimal](15, 4) NULL,
	            [TotalSum] [decimal](15, 4) NULL,
	            [InvoiceDate] [date] NULL,
	            [DueDate] [date] NULL,
                CONSTRAINT [PK_Invoice] PRIMARY KEY CLUSTERED ([Id] ASC));
            CREATE TABLE [dbo].[Order](
	            [Id] [int] IDENTITY(1,1) NOT NULL,
	            [InvoiceId] [int] NULL,
	            [OrderType] [varchar](50) NULL,
	            [OrderDate] [datetime] NULL,
	            [CompanySellerId] [int] NULL,
	            [CompanyBuyerId] [int] NULL,
	            [Amount] [decimal](15, 3) NOT NULL,
	            [Gtin] [uniqueidentifier] NULL,
	            [ArticleName] [varchar](50) NULL,
	            [UnitPrice] [decimal](15, 4) NULL,
	            [UnitType] [varchar](50) NULL,
                CONSTRAINT [PK_Order] PRIMARY KEY CLUSTERED ([Id] ASC)); 
                ";

        public static string alterDb = @"
        ALTER TABLE [dbo].[Article]  WITH CHECK ADD  CONSTRAINT [FK_Article_CompanySeller] FOREIGN KEY([CompanySellerId])REFERENCES [dbo].[CompanySeller] ([Id]);
        ALTER TABLE [dbo].[Article] CHECK CONSTRAINT [FK_Article_CompanySeller];
        ALTER TABLE [dbo].[Order]  WITH CHECK ADD  CONSTRAINT [FK_Order_CompanyBuyer] FOREIGN KEY([CompanyBuyerId]) REFERENCES [dbo].[CompanyBuyer] ([Id]);
	    ALTER TABLE [dbo].[Order] CHECK CONSTRAINT [FK_Order_CompanyBuyer]
        ALTER TABLE [dbo].[Order]  WITH CHECK ADD  CONSTRAINT [FK_Order_CompanySeller] FOREIGN KEY([CompanySellerId]) REFERENCES [dbo].[CompanySeller] ([Id]);
        ALTER TABLE [dbo].[Order] CHECK CONSTRAINT [FK_Order_CompanySeller];
        ALTER TABLE [dbo].[Order]  WITH CHECK ADD  CONSTRAINT [FK_Order_Invoice] FOREIGN KEY([InvoiceId]) REFERENCES [dbo].[Invoice] ([Id]);
        ALTER TABLE [dbo].[Order] CHECK CONSTRAINT [FK_Order_Invoice];
        ";


        public static string insertDb = @"
            INSERT [dbo].[CompanyBuyer] ([Name], [OrgNo], [City]) VALUES (N'Skogsblomman AB', N'661111-1111', N'Stockholm');
            INSERT [dbo].[CompanyBuyer] ([Name], [OrgNo], [City]) VALUES (N'AD Consulting AB', N'662222-2222', N'Stockholm');

            INSERT [dbo].[CompanySeller] ([Name], [OrgNo], [City]) VALUES (N'Bröderna Miller AB', N'551111-1111', N'Göteborg');
            INSERT [dbo].[CompanySeller] ([Name], [OrgNo], [City]) VALUES (N'Salladsfabriken AB', N'552222-2222', N'Göteborg');
            INSERT [dbo].[CompanySeller] ([Name], [OrgNo], [City]) VALUES (N'Lakritskungen AB', N'553333-3333', N'Göteborg');

            INSERT [dbo].[Article] ([ArticleName], [Gtin], [CompanySellerId], [UnitPrice], [UnitType]) 
            VALUES (N'A-Vitamin', N'1c30b175-ca5c-4a6c-a997-1590a7d790e5', 1, CAST(89.0000 AS Decimal(15, 4)), N'1');
            INSERT [dbo].[Article] ([ArticleName], [Gtin], [CompanySellerId], [UnitPrice], [UnitType]) 
            VALUES (N'B-Vitamin', N'b8d3b054-89e3-4fea-8e61-fbf2a937cbf3', 1, CAST(79.0000 AS Decimal(15, 4)), N'1');
            INSERT [dbo].[Article] ([ArticleName], [Gtin], [CompanySellerId], [UnitPrice], [UnitType]) 
            VALUES (N'D-Vitamin', N'ba3f0e5e-b7cf-4b45-85ea-a71e2db0e12c', 1, CAST(84.0000 AS Decimal(15, 4)), N'1');
            INSERT [dbo].[Article] ([ArticleName], [Gtin], [CompanySellerId], [UnitPrice], [UnitType]) 
            VALUES (N'Kurkummin', N'28fb3aec-b717-4246-974c-ec1c614a2985', 1, CAST(99.0000 AS Decimal(15, 4)), N'1');
            INSERT [dbo].[Article] ([ArticleName], [Gtin], [CompanySellerId], [UnitPrice], [UnitType]) 
            VALUES (N'Laktrisrot', N'e0f36709-18d2-4dde-b913-bc5e812e20cf', 3, CAST(469.0000 AS Decimal(15, 4)), N'kg');
            INSERT [dbo].[Article] ([ArticleName], [Gtin], [CompanySellerId], [UnitPrice], [UnitType]) 
            VALUES (N'Hallonte', N'dd378715-64a5-4690-9494-f6b9f3c4f595', 3, CAST(0.3990 AS Decimal(15, 4)), N'gram');
            INSERT [dbo].[Article] ([ArticleName], [Gtin], [CompanySellerId], [UnitPrice], [UnitType]) 
            VALUES (N'Vitkål', N'8d1e3abd-58b8-48b4-a177-51be8e53b6ca', 2, CAST(7.8900 AS Decimal(15, 4)), N'kg');
            INSERT [dbo].[Article] ([ArticleName], [Gtin], [CompanySellerId], [UnitPrice], [UnitType]) 
            VALUES (N'Rödbetor', N'19ac7025-32e2-469e-8dd4-403e0f3c39ca', 2, CAST(7.8900 AS Decimal(15, 4)), N'kg');
            INSERT [dbo].[Article] ([ArticleName], [Gtin], [CompanySellerId], [UnitPrice], [UnitType]) 
            VALUES (N'Morötter', N'c30aad2b-07f9-493b-81e0-b583097ec1d4', 2, CAST(6.8900 AS Decimal(15, 4)), N'kg');

            INSERT [dbo].[Order] ([InvoiceId], [OrderType], [OrderDate], [CompanySellerId], [CompanyBuyerId], [Amount], [Gtin], [ArticleName], [UnitPrice], [UnitType]) VALUES (NULL, N'280', CAST(N'2015-03-20 15:30:25.280' AS DateTime), 1, 1, CAST(10.000 AS Decimal(15, 3)), N'8878c19a-18c3-4fd4-90a0-53b22679ef18', N'A-Vitamin', CAST(89.0000 AS Decimal(15, 4)), N'1');
	        INSERT [dbo].[Order] ([InvoiceId], [OrderType], [OrderDate], [CompanySellerId], [CompanyBuyerId], [Amount], [Gtin], [ArticleName], [UnitPrice], [UnitType]) VALUES (NULL, N'280', CAST(N'2015-03-20 15:30:46.930' AS DateTime), 1, 1, CAST(10.000 AS Decimal(15, 3)), N'dc55ed1f-3770-463d-b213-59a6f8dd3d93', N'B-Vitamin', CAST(79.0000 AS Decimal(15, 4)), N'1');
	        INSERT [dbo].[Order] ([InvoiceId], [OrderType], [OrderDate], [CompanySellerId], [CompanyBuyerId], [Amount], [Gtin], [ArticleName], [UnitPrice], [UnitType]) VALUES (NULL, N'280', CAST(N'2015-03-20 15:31:08.787' AS DateTime), 1, 2, CAST(20.000 AS Decimal(15, 3)), N'0bff3f4b-72ea-490f-9b7c-b1a655cf65aa', N'A-Vitamin', CAST(89.0000 AS Decimal(15, 4)), N'1');
	        INSERT [dbo].[Order] ([InvoiceId], [OrderType], [OrderDate], [CompanySellerId], [CompanyBuyerId], [Amount], [Gtin], [ArticleName], [UnitPrice], [UnitType]) VALUES (NULL, N'280', CAST(N'2015-03-20 15:31:36.783' AS DateTime), 1, 2, CAST(20.000 AS Decimal(15, 3)), N'd4e97a2d-7d64-49d2-a22c-9c40babca6c9', N'D-Vitamin', CAST(84.0000 AS Decimal(15, 4)), N'1');
	        INSERT [dbo].[Order] ([InvoiceId], [OrderType], [OrderDate], [CompanySellerId], [CompanyBuyerId], [Amount], [Gtin], [ArticleName], [UnitPrice], [UnitType]) VALUES (NULL, N'280', CAST(N'2015-03-20 15:32:15.910' AS DateTime), 2, 1, CAST(100.000 AS Decimal(15, 3)), N'592334bf-941d-488a-803a-676b24790a29', N'Morötter', CAST(6.8900 AS Decimal(15, 4)), N'kg');
	        INSERT [dbo].[Order] ([InvoiceId], [OrderType], [OrderDate], [CompanySellerId], [CompanyBuyerId], [Amount], [Gtin], [ArticleName], [UnitPrice], [UnitType]) VALUES (NULL, N'280', CAST(N'2015-03-20 15:32:40.550' AS DateTime), 2, 1, CAST(88.000 AS Decimal(15, 3)), N'ef67f0b7-cb52-4659-af09-8a4cf389729c', N'Rödbetor', CAST(7.8900 AS Decimal(15, 4)), N'kg');
	        INSERT [dbo].[Order] ([InvoiceId], [OrderType], [OrderDate], [CompanySellerId], [CompanyBuyerId], [Amount], [Gtin], [ArticleName], [UnitPrice], [UnitType]) VALUES (NULL, N'280', CAST(N'2015-03-20 15:33:31.140' AS DateTime), 3, 2, CAST(2000.000 AS Decimal(15, 3)), N'4184b223-2d83-4340-8362-52213b3a0e89', N'Hallonte', CAST(0.4000 AS Decimal(15, 4)), N'gram');
	        INSERT [dbo].[Order] ([InvoiceId], [OrderType], [OrderDate], [CompanySellerId], [CompanyBuyerId], [Amount], [Gtin], [ArticleName], [UnitPrice], [UnitType]) VALUES (NULL, N'280', CAST(N'2015-03-20 15:34:05.507' AS DateTime), 3, 2, CAST(2.000 AS Decimal(15, 3)), N'7ddf8e23-8629-475e-ab4c-afe77f360773', N'Laktrisrot', CAST(469.0000 AS Decimal(15, 4)), N'kg');
	        INSERT [dbo].[Order] ([InvoiceId], [OrderType], [OrderDate], [CompanySellerId], [CompanyBuyerId], [Amount], [Gtin], [ArticleName], [UnitPrice], [UnitType]) VALUES (NULL, N'280', CAST(N'2015-03-20 15:34:31.570' AS DateTime), 2, 1, CAST(40.000 AS Decimal(15, 3)), N'c2579b55-a259-4b20-8d37-bc9d9ee09e4c', N'Vitkål', CAST(7.8900 AS Decimal(15, 4)), N'kg');
	        INSERT [dbo].[Order] ([InvoiceId], [OrderType], [OrderDate], [CompanySellerId], [CompanyBuyerId], [Amount], [Gtin], [ArticleName], [UnitPrice], [UnitType]) VALUES (NULL, N'280', CAST(N'2015-03-20 15:35:02.090' AS DateTime), 2, 1, CAST(22.000 AS Decimal(15, 3)), N'aa8383aa-b641-4ba9-9dd3-481be46afb5e', N'Morötter', CAST(6.8900 AS Decimal(15, 4)), N'kg');
	        INSERT [dbo].[Order] ([InvoiceId], [OrderType], [OrderDate], [CompanySellerId], [CompanyBuyerId], [Amount], [Gtin], [ArticleName], [UnitPrice], [UnitType]) VALUES (NULL, N'280', CAST(N'2015-03-20 15:35:25.507' AS DateTime), 3, 1, CAST(3000.000 AS Decimal(15, 3)), N'd7b9a293-e3a8-4df5-9697-34319f8664d8', N'Hallonte', CAST(0.4000 AS Decimal(15, 4)), N'gram');
	        INSERT [dbo].[Order] ([InvoiceId], [OrderType], [OrderDate], [CompanySellerId], [CompanyBuyerId], [Amount], [Gtin], [ArticleName], [UnitPrice], [UnitType]) VALUES (NULL, N'280', CAST(N'2015-03-20 15:35:58.560' AS DateTime), 1, 2, CAST(15.000 AS Decimal(15, 3)), N'c0b3cdf1-1017-49d4-95d4-14b57a9e42ca', N'B-Vitamin', CAST(79.0000 AS Decimal(15, 4)), N'1');
	        INSERT [dbo].[Order] ([InvoiceId], [OrderType], [OrderDate], [CompanySellerId], [CompanyBuyerId], [Amount], [Gtin], [ArticleName], [UnitPrice], [UnitType]) VALUES (NULL, N'280', CAST(N'2015-03-20 15:36:47.183' AS DateTime), 2, 2, CAST(14.180 AS Decimal(15, 3)), N'b80cc454-9406-4d92-8abd-c89c1fddc4d9', N'Morötter', CAST(6.8900 AS Decimal(15, 4)), N'kg');
	        INSERT [dbo].[Order] ([InvoiceId], [OrderType], [OrderDate], [CompanySellerId], [CompanyBuyerId], [Amount], [Gtin], [ArticleName], [UnitPrice], [UnitType]) VALUES (NULL, N'280', CAST(N'2015-03-20 15:37:17.647' AS DateTime), 3, 2, CAST(2.230 AS Decimal(15, 3)), N'aca2e04f-ad6b-450b-9213-4cae3372b693', N'Laktrisrot', CAST(469.0000 AS Decimal(15, 4)), N'kg');
	        INSERT [dbo].[Order] ([InvoiceId], [OrderType], [OrderDate], [CompanySellerId], [CompanyBuyerId], [Amount], [Gtin], [ArticleName], [UnitPrice], [UnitType]) VALUES (NULL, N'280', CAST(N'2015-03-20 15:37:49.187' AS DateTime), 2, 2, CAST(11.130 AS Decimal(15, 3)), N'3ca94fba-8802-42cb-83dc-9d78833c67ac', N'Morötter', CAST(6.8900 AS Decimal(15, 4)), N'kg');
	        INSERT [dbo].[Order] ([InvoiceId], [OrderType], [OrderDate], [CompanySellerId], [CompanyBuyerId], [Amount], [Gtin], [ArticleName], [UnitPrice], [UnitType]) VALUES (NULL, N'280', CAST(N'2015-03-20 15:38:07.950' AS DateTime), 1, 1, CAST(15.000 AS Decimal(15, 3)), N'1d09496b-f47d-46be-8224-d744a94913a5', N'A-Vitamin', CAST(89.0000 AS Decimal(15, 4)), N'1');
	        INSERT [dbo].[Order] ([InvoiceId], [OrderType], [OrderDate], [CompanySellerId], [CompanyBuyerId], [Amount], [Gtin], [ArticleName], [UnitPrice], [UnitType]) VALUES (NULL, N'280', CAST(N'2015-03-20 15:38:36.650' AS DateTime), 2, 2, CAST(18.550 AS Decimal(15, 3)), N'70829875-9b56-4abb-9051-8b413a79cb5a', N'Morötter', CAST(6.8900 AS Decimal(15, 4)), N'kg');";
        }

    public class DbHelper
    {
        private SqlConnection _connection;

        public DbHelper()
        {
            _connection = new SqlConnection(ConnectionString());
        }

        public void Open()
        {
            _connection.Open();
        }

        public void Close()
        {
            _connection.Close();
        }

        public SqlConnection GetConnection()
        {
            return _connection;
        }

        private string ConnectionString()
        {
            string connStr = ConfigurationManager.ConnectionStrings["DefaultConnection"].ToString();
            SqlConnectionStringBuilder sb = new SqlConnectionStringBuilder(connStr);

            sb.ApplicationName = "Min applikation";
            sb.ConnectTimeout = 30;

            return sb.ToString();
        }

        public IEnumerable<T> GetAll<T>(string view = "")
        {
            var type = typeof(T);

            var command = _connection.CreateCommand();
            command.CommandText = string.IsNullOrEmpty(view) ? "SELECT * FROM " + type.Name : command.CommandText = "SELECT * FROM [" + view + "]";
            _connection.Open();
            var dataReader = command.ExecuteReader();
            var list = new List<T>();
            while (dataReader.Read())
            {
                var instance = (T)Activator.CreateInstance(type);
                foreach (PropertyInfo propertyInfo in type.GetProperties())
                {
                    ExcludeNull<T>(dataReader, instance, propertyInfo);
                }
                list.Add(instance);
            }
            _connection.Close();
            return list;
        }

        public IEnumerable<T> GetById<T>(string id)
        {
            var type = typeof(T);

            var command = _connection.CreateCommand();
            command.CommandText = "SELECT * FROM " + type.Name + " WHERE Id = " + id;
            _connection.Open();
            var dataReader = command.ExecuteReader();
            var list = new List<T>();
            while (dataReader.Read())
            {
                var instance = (T)Activator.CreateInstance(type);
                foreach (var propertyInfo in type.GetProperties())
                {
                    ExcludeNull<T>(dataReader, instance, propertyInfo);
                }
                list.Add(instance);
            }
            _connection.Close();
            return list;
        }

        //Hanterar nullvärden 
        private static void ExcludeNull<T>(SqlDataReader dataReader, T instance, PropertyInfo propertyInfo)
        {
            if (propertyInfo.PropertyType.Equals(typeof(string)))
            {
                if (dataReader[propertyInfo.Name].ToString().Equals(string.Empty))
                { propertyInfo.SetValue(instance, string.Empty); }
                else
                { propertyInfo.SetValue(instance, dataReader[propertyInfo.Name]); }
            }
            else if (propertyInfo.PropertyType.Equals(typeof(int)))
            {
                if (dataReader[propertyInfo.Name].ToString().Equals(string.Empty))
                { propertyInfo.SetValue(instance, 0); }
                else
                { propertyInfo.SetValue(instance, dataReader[propertyInfo.Name]); }
            }
            else
            {
                propertyInfo.SetValue(instance, dataReader[propertyInfo.Name]);
            }
        }
    }
}