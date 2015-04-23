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

            if (!context.Users.Any(u => u.Email == "helena.robinson@_ica.se"))
            {
                var user = new ApplicationUser { RegisterCompanyName = "ICA NÄRA Örebro", Email = "helena.robinson@_ica.se", UserName = "helena.robinson@_ica.se" };
                userManager.Create(user, "qqqq1!Q");
            }

            if (!context.Users.Any(u => u.Email == "laura.andrews@_willys.se"))
            {
                var user = new ApplicationUser { RegisterCompanyName = "Willy's strand", Email = "laura.andrews@_willys.se", UserName = "laura.andrews@_willys.se" };
                userManager.Create(user, "qqqq1!Q");
            }

            if (!context.Users.Any(u => u.Email == "maria.andersson@_ica.se"))
            {
                var user = new ApplicationUser { RegisterCompanyName = "ICA kvantum", Email = "maria.andersson@_ica.se", UserName = "maria.andersson@_ica.se" };
                userManager.Create(user, "qqqq1!Q");
            }

            if (!context.Users.Any(u => u.Email == "susan.grant@_volvocars.com"))
            {
                var user = new ApplicationUser { RegisterCompanyName = "Torslanda", Email = "susan.grant@_volvocars.com", UserName = "susan.grant@_volvocars.com" };
                userManager.Create(user, "qqqq1!Q");
            }

            if (!context.Users.Any(u => u.Email == "edgar.stevens@_willys.se"))
            {
                var user = new ApplicationUser { RegisterCompanyName = "Willis", Email = "edgar.stevens@_willys.se", UserName = "edgar.stevens@_willys.se" };
                userManager.Create(user, "qqqq1!Q");
            }

            if (!context.Users.Any(u => u.Email == "jason.hall@_ica.se"))
            {
                var user = new ApplicationUser { RegisterCompanyName = "Fokus (Ica)", Email = "jason.hall@_ica.se", UserName = "jason.hall@_ica.se" };
                userManager.Create(user, "qqqq1!Q");
            }

            if (!context.Users.Any(u => u.Email == "bertil.clark@_volvocars.com"))
            {
                var user = new ApplicationUser { RegisterCompanyName = "Pinifarina", Email = "bertil.clark@_volvocars.com", UserName = "bertil.clark@_volvocars.com" };
                userManager.Create(user, "qqqq1!Q");
            }

            if (!context.Users.Any(u => u.Email == "sandra.lewis@_volvocars.com"))
            {
                var user = new ApplicationUser { RegisterCompanyName = "tuve", Email = "sandra.lewis@_volvocars.com", UserName = "sandra.lewis@_volvocars.com" };
                userManager.Create(user, "qqqq1!Q");
            }
        }
    }
}
