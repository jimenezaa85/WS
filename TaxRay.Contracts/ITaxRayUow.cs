using System;
using TaxRay.Model;

namespace TaxRay.Contracts
{
    /// <summary>
    /// Interface for the Tax Ray "Unit of Work"
    /// </summary>
    public interface ITaxRayUow: IDisposable
    {
        // Save pending changes to the data store.
        void Commit();

        // Repositories
        ITaxReturnRepository Taxes { get; }
        IRepository<User> Users { get; }

    }
}