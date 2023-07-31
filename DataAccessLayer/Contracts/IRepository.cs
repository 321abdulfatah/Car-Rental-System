using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccessLayer.Contracts
{
    public interface IRepository <T>
    {
        public T Create(T _object);
        public void Delete(T _object);
        public Task<T> Update(T _object);
        public IEnumerable<T> GetAll();

        public IEnumerable<T> GetAll(string Type);

        public IEnumerable<T> GetAll(int page, int pageSize);

        public T GetById(Guid Id);
    }
}
