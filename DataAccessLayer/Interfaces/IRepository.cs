using DataAccessLayer.Models;

namespace DataAccessLayer.Interfaces
{
    public interface IRepository <T> where T : BaseModel
    {
        public Task<T> Create(T _object);
        public T Delete(Guid Id);
        public Task<T> Update(T _object);
        public IQueryable<T> GetList();
        public T Get(Guid Id);
    }
}
