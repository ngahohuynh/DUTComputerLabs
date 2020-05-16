using System.Collections.Generic;
using System.Threading.Tasks;

namespace DUTComputerLabs.API.Repositories
{
    public interface IRepository<T> where T : class
    {
        IEnumerable<T> GetAll();

        T GetById(int id);

        void Add(T entity);

        void Delete(T entity);

        // bool Exists(int id);

        Task<bool> SaveAll();
         
    }
}