using DataAccessLayer.Common.Models;
using DataAccessLayer.Models;

namespace DataAccessLayer.Interfaces
{
    public interface IRepository<T> where T : BaseModel
    {
        Task<T> Create(T entity);
        void Delete(T entity);
        void Update(T entity);
        Task<IEnumerable<T>> GetList();
        Task<T> Get(Guid Id);
    }
}