using System;
using System.Collections.Generic;
using TaxRay.Model;

namespace TaxRay.Contracts
{
    public interface ITaxReturnRepository : IRepository<TaxReturn>
    {
        /// <summary>
        /// Retrieves tasks assigned to a given user
        /// </summary>
        /// <remarks>
        /// We request specifically the user name, not the whole User instance
        /// to delegate the validation to the caller
        /// </remarks>
        /// <param name="userName">Username</param>
        /// <returns>Collections of <see cref="TaxReturn"/></returns>
        IEnumerable<TaxReturn> GetAssignedTasks(string userName);
        IEnumerable<TaxReturn> GetAssignedTasksSuperAdmin();

        /// <summary>
        /// procedure to assign task an user
        /// </summary>
        /// <param name="taxId">Identifier tax</param>
        /// <param name="currentId">Identifier user logged</param>
        /// <param name="assignUserId">Identifier user assign to task</param>
        void Assign(int taxId, int currentId, int? assignUserId);

        void UpdateTax(int taxId, string description, string client, string taxPayer, int year, DateTime dueDate, string status);
        


        
    }
}