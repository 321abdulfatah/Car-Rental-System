using DataAccessLayer.Common.Models;
using DataAccessLayer.Models;

namespace DataAccessLayer.Interfaces
{
    public interface IRepository<T> where T : BaseModel
    {
        public Task<T> Create(T obj);
        public Task<T> Delete(Guid Id);
        public Task<T> Update(T obj);
        public PaginatedResult<T> GetList(string search, string column, string sortOrder, string orderBy, int pageIndex, int pageSize);
        public Task<T> Get(Guid Id);
    }
}