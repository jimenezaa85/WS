using System.Linq;

namespace TaxRay.Contracts
{
    public interface IRepository<T> where T : class
    {
        IQueryable<T> GetAll();
        T GetById(int id);
        T Add(T entity);
        void Update(T entity);
        void Delete(T entity);
        void Delete(int id);
    }
}