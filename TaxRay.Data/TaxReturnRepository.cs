using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using TaxRay.Contracts;
using TaxRay.Model;

using System;
namespace TaxRay.Data
{
    public class TaxReturnRepository : EntityFrameworkRepository<TaxReturn>, ITaxReturnRepository
    {
        public TaxReturnRepository(DbContext context) : base(context) { }

        public IEnumerable<TaxReturn> GetAssignedTasks(string userName)
        {
            return GetAll()
                .Where(t => t.AssignedTo != null &&
                            t.AssignedTo.Username == userName)
                .Include(e => e.AssignedTo)
                .Include(e => e.CreatedBy);
        }

        public IEnumerable<TaxReturn> GetAssignedTasksSuperAdmin()
        {
            return GetAll()
                .Where(t => t.AssignedTo != null)
                .Include(e => e.AssignedTo)
                .Include(e => e.CreatedBy)
                .Union(GetAll().Where(t => t.AssignedTo ==null));
                
        }

        public void Assign(int taxId, int currentId, int? assignUserId)
        {
            var tax = DbSet.Single(t => t.Id == taxId);
            tax.AssignedToId = assignUserId;
            tax.CreatedById = currentId;

            DbContext.SaveChanges();
        }

        public void UpdateTax(int taxId, string description, string client, string taxPayer, int year, DateTime dueDate, string status)
        {
            var tax = DbSet.Single(t => t.Id == taxId);
            tax.Description = description;
            tax.Client = client;
            tax.TaxPayer = taxPayer;
            tax.Year = year;
            tax.DueDate = dueDate;
            tax.Status = status;

            DbContext.SaveChanges();
        }

        public override TaxReturn GetById(int id)
        {
            return DbSet.Where(c => c.Id == id).Include(e => e.AssignedTo).Include(e => e.CreatedBy).FirstOrDefault();
        }

    }
}