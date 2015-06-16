using System.Data.Entity.Migrations;
using System.Linq;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using TaxRay.Models;

namespace TaxRay.Migrations
{
    internal sealed class Configuration : DbMigrationsConfiguration<ApplicationDbContext>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = false;
        }

        protected override void Seed(ApplicationDbContext context)
        {

            if (context.Users.Any(u => u.UserName == "glang"))
            {
                return;
            }

            CreateAndAssign(context, "glang", "SuperAdmin");
            CreateAndAssign(context, "zwillis", "Senior Preparer");
            CreateAndAssign(context, "mglenn", "Junior Preparer");
            CreateAndAssign(context, "pblopez", "Tax Creator");
        }

        const string DEFAULT_PASSWORD = "Secret2015";


        private void CreateAndAssign(ApplicationDbContext context, string username, string role)
        {
            var roleStore = new RoleStore<IdentityRole>(context);
            var roleManager = new RoleManager<IdentityRole>(roleStore);

            if (roleManager.FindByName(role) == null)
            {
                roleManager.Create(new IdentityRole { Name = role });
            }

            var userStore = new UserStore<ApplicationUser>(context);
            var userManager = new UserManager<ApplicationUser>(userStore);

            var user = new ApplicationUser { UserName = username, Email = username + "@kpmg.com" };
            userManager.Create(user, DEFAULT_PASSWORD);
            userManager.AddToRole(user.Id, role);
        }
    }
}