using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessAccessLayer.Services
{
    public interface IServiceCar <T>
    {
        public T Create(T _object);
        public void Delete(int Id);

        public Task<T> Update(T _object);

        public IEnumerable<T> GetAll();

        public IEnumerable<T> GetAll(string Type);

        public IEnumerable<T> GetAll(int page, int pageSize);

        public T GetById(int Id);

    }
}
