namespace ErlezWebUI.Migrations
{
    using ErlezWebUI.Models;
    using Microsoft.AspNet.Identity;
    using Microsoft.AspNet.Identity.EntityFramework;
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Migrations;
    using System.Linq;

    internal sealed class Configuration : DbMigrationsConfiguration<ErlezWebUI.Models.ApplicationDbContext>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = true;
            AutomaticMigrationDataLossAllowed = true;
        }

        protected override void Seed(ErlezWebUI.Models.ApplicationDbContext context)
        {
            SeedUsers(context);
        }

        private static void SeedUsers(ErlezWebUI.Models.ApplicationDbContext context)
        {
            context.CompanyB2s.AddOrUpdate(
                c => c.CompanyName,
                new CompanyB2 { CompanyName = "Edi Solutions" },
                new CompanyB2 { CompanyName = "Volvo Personbilar" },
                new CompanyB2 { CompanyName = "Volvo Lastvagnar" },
                new CompanyB2 { CompanyName = "Ica" },
                new CompanyB2 { CompanyName = "Willys" },
                new CompanyB2 { CompanyName = "Netto" }
                );

            if (!context.Roles.Any(r => r.Name == "CanEditUser"))
            {
                var roleStore = new RoleStore<IdentityRole>(context);
                var roleManager = new RoleManager<IdentityRole>(roleStore);
                roleManager.Create(new IdentityRole { Name = "CanEditUser" });
            }

            var userStore = new UserStore<ApplicationUser>(context);
            var userManager = new UserManager<ApplicationUser>(userStore);

            if (!context.Users.Any(u => u.Email == "admin@edisolutions.se"))
            {
                var user = new ApplicationUser { CompanyId = 99, Email = "admin@edisolutions.se", UserName = "admin@edisolutions.se" };
                userManager.Create(user, "qqqq1!Q");
                userManager.AddToRole(user.Id, "CanEditUser");
            }

            if (!context.Users.Any(u => u.Email == "helena.robinson@ica.se"))
            {
                var user = new ApplicationUser { RegisterCompanyName = "ICA NÄRA Örebro", Email = "helena.robinson@ica.se", UserName = "helena.robinson@ica.se" };
                userManager.Create(user, "qqqq1!Q");
            }

            if (!context.Users.Any(u => u.Email == "laura.andrews@willys.se"))
            {
                var user = new ApplicationUser { RegisterCompanyName = "Willy's strand", Email = "laura.andrews@willys.se", UserName = "laura.andrews@willys.se" };
                userManager.Create(user, "qqqq1!Q");
            }

            if (!context.Users.Any(u => u.Email == "maria.andersson@ica.se"))
            {
                var user = new ApplicationUser { RegisterCompanyName = "ICA kvantum", Email = "maria.andersson@ica.se", UserName = "maria.andersson@ica.se" };
                userManager.Create(user, "qqqq1!Q");
            }

            if (!context.Users.Any(u => u.Email == "susan.grant@volvocars.com"))
            {
                var user = new ApplicationUser { RegisterCompanyName = "Torslanda", Email = "susan.grant@volvocars.com", UserName = "susan.grant@volvocars.com" };
                userManager.Create(user, "qqqq1!Q");
            }

            if (!context.Users.Any(u => u.Email == "edgar.stevens@willys.se"))
            {
                var user = new ApplicationUser { RegisterCompanyName = "Willis", Email = "edgar.stevens@willys.se", UserName = "edgar.stevens@willys.se" };
                userManager.Create(user, "qqqq1!Q");
            }

            if (!context.Users.Any(u => u.Email == "jason.hall@ica.se"))
            {
                var user = new ApplicationUser { RegisterCompanyName = "Fokus (Ica)", Email = "jason.hall@ica.se", UserName = "jason.hall@ica.se" };
                userManager.Create(user, "qqqq1!Q");
            }

            if (!context.Users.Any(u => u.Email == "bertil.clark@volvocars.com"))
            {
                var user = new ApplicationUser { RegisterCompanyName = "Pinifarina", Email = "bertil.clark@volvocars.com", UserName = "bertil.clark@volvocars.com" };
                userManager.Create(user, "qqqq1!Q");
            }

            if (!context.Users.Any(u => u.Email == "sandra.lewis@volvocars.com"))
            {
                var user = new ApplicationUser { RegisterCompanyName = "tuve", Email = "sandra.lewis@volvocars.com", UserName = "sandra.lewis@volvocars.com" };
                userManager.Create(user, "qqqq1!Q");
            }
        }
    }
}
