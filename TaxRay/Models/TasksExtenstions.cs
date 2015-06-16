using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;

namespace TaxRay.Models
{
    public static class TasksExtenstions
    {
        public static IQueryable<TaxReturn> GetAssignedTasks(this DbSet<TaxReturn> repo, string userName)
        {
            return repo
                            .Where(t => t.AssignedTo != null &&
                                        t.AssignedTo.UserName == userName)
                            .Include(e => e.AssignedTo)
                            .Include(e => e.CreatedBy);

        }

        public static void Assign(this DbSet<TaxReturn> repo, int taxId, string currentId, string assignUserId)
        {
            var tax = repo.Single(t => t.Id == taxId);
            tax.AssignedToId = assignUserId;
            tax.CreatedById = currentId;
        }

        public static TaxReturn GetById(this DbSet<TaxReturn> repo, int id)
        {
            return repo.Where(c => c.Id == id).Include(e => e.AssignedTo).Include(e => e.CreatedBy).FirstOrDefault();
        }

        public static void Update(this DbSet<TaxReturn> repo, TaxReturn entity, ApplicationDbContext context)
        {
            DbEntityEntry dbEntityEntry = context.Entry(entity);
            if (dbEntityEntry.State == EntityState.Detached)
            {
                repo.Attach(entity);
            }
            dbEntityEntry.State = EntityState.Modified;
        }

        public static void Delete(this DbSet<TaxReturn> repo, TaxReturn entity, ApplicationDbContext context)
        {
            DbEntityEntry dbEntityEntry = context.Entry(entity);
            if (dbEntityEntry.State != EntityState.Deleted)
            {
                dbEntityEntry.State = EntityState.Deleted;
            }
            else
            {
                repo.Attach(entity);
                repo.Remove(entity);
            }
        }

        public static void Delete(this DbSet<TaxReturn> repo, int id, ApplicationDbContext context)
        {
            var entity = repo.Find(id);
            if (entity == null) return; // not found; assume already deleted.
            Delete(repo, entity, context);
        }
    }
}