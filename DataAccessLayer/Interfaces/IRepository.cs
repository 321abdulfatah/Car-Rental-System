using DataAccessLayer.Models;

namespace DataAccessLayer.Interfaces
{
    public interface IRepository <T> where T : BaseModel
    {
        public T Create(T _object);
        public void Delete(Guid Id);
        public Task<T> Update(T _object);
        public IEnumerable<T> GetAll();
        public IEnumerable<T> GetAll(int page, int pageSize);
        public T GetById(Guid Id);
    }
}
