namespace control_notas_cit.Migrations
{
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Migrations;
    using System.Linq;
    using Microsoft.AspNet.Identity;
    using Microsoft.AspNet.Identity.EntityFramework;
    using IdentitySample.Models;

    internal sealed class Configuration : DbMigrationsConfiguration<IdentitySample.Models.ApplicationDbContext>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = true;
            AutomaticMigrationDataLossAllowed = true;
        }

        protected override void Seed(IdentitySample.Models.ApplicationDbContext context)
        {
            ApplicationUserManager userMgr = new ApplicationUserManager(new UserStore<ApplicationUser>(context));
            ApplicationRoleManager roleMgr = new ApplicationRoleManager(new RoleStore<IdentityRole>(context));

            string roleName = "Admin";
            string coordinadorRole = "Coordinador";
            string profesorRole = "Profesor";

            if (!roleMgr.RoleExists(roleName))
            {
                roleMgr.Create(new IdentityRole(roleName));
            }

            if (!roleMgr.RoleExists(coordinadorRole))
            {
                roleMgr.Create(new IdentityRole(coordinadorRole));
            }

            if (!roleMgr.RoleExists(profesorRole))
            {
                roleMgr.Create(new IdentityRole(profesorRole));
            }

            //// Admin
            string userName = "admin@cit.com";
            string password = "cit2015";
            string email = "admin@cit.com";

            ApplicationUser user = userMgr.FindByName(userName);

            if (user == null)
            {
                IdentityResult result = userMgr.Create(new ApplicationUser { UserName = userName, Email = email }, password);
                user = userMgr.FindByName(userName);
            }

            if (!userMgr.IsInRole(user.Id, roleName))
            {
                userMgr.AddToRole(user.Id, roleName);
            }

            context.SaveChanges();
        }
    }
}
