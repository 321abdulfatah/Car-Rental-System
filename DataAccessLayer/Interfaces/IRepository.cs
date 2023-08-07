using DataAccessLayer.Common.Models;
using DataAccessLayer.Models;

namespace DataAccessLayer.Interfaces
{
    public interface IRepository <T> where T : BaseModel
    {
        public Task<T> Create(T _object);
        public Task<T> Delete(Guid Id);
        public Task<T> Update(T _object);
        public PaginatedResult<T> GetList(string Search, string Column, string SortOrder, string OrderBy, int PageIndex, int PageSize);
        public T Get(Guid Id);
    }
}
