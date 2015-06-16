using System;
using System.Data.Entity;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;

namespace TaxRay.Models
{
    // You can add profile data for the user by adding more properties to your ApplicationUser class, please visit http://go.microsoft.com/fwlink/?LinkID=317594 to learn more.
    public class ApplicationUser : IdentityUser
    {
        public async Task<ClaimsIdentity> GenerateUserIdentityAsync(UserManager<ApplicationUser> manager, string authenticationType)
        {
            // Note the authenticationType must match the one defined in CookieAuthenticationOptions.AuthenticationType
            var userIdentity = await manager.CreateIdentityAsync(this, authenticationType);
            // Add custom user claims here
            return userIdentity;
        }
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

        public DbSet<TaxReturn> Tasks { get; set; }
    }

    public class TaxReturn
    {
        public int? Id { get; set; }
        public string AtlasId { get; set; }
        public string Description { get; set; }
        public string Client { get; set; }
        public string TaxPayer { get; set; }
        public DateTime? DueDate { get; set; }
        public string Status { get; set; }
        public int Year { get; set; }
        public string AssignedToId { get; set; }
        public string CreatedById { get; set; }
        public virtual ApplicationUser AssignedTo { get; set; }
        public virtual ApplicationUser CreatedBy { get; set; }
    }
}