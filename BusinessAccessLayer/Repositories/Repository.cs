using DataAccessLayer.Models;
using DataAccessLayer.Interfaces;
using DataAccessLayer.Data;
using Microsoft.EntityFrameworkCore;

namespace BusinessAccessLayer.Repositories
{
    public class Repository<T> : IRepository<T> where T : BaseModel
    {
        private readonly CarRentalDBContext _CarRentalDBContext;
        private readonly DbSet<T> _entities;

        public Repository(CarRentalDBContext context)
        {
            _CarRentalDBContext = context ?? throw new ArgumentNullException(nameof(context));
            _entities = context.Set<T>();
        }

        public T Create(T _object)
        {
            if (_object == null)
            {
                throw new ArgumentNullException("entity");
            }
            _entities.Add(_object);
            _CarRentalDBContext.SaveChanges();

            return _object;
        }

        public void Delete(T _object)
        {
            if (_object == null)
            {
                throw new ArgumentNullException("entity");
            }
            _entities.Remove(_object);
            _CarRentalDBContext.SaveChanges();
        }

        public IEnumerable<T> GetAll()
        {
            try
            {
                return _entities.AsEnumerable();
            }
            catch (Exception)
            {
                throw;
            }
        }

        public IEnumerable<T> GetAll(int page, int pageSize)
        {
            try
            {
                int totalNumbers = page * pageSize;

                return _entities.Skip(totalNumbers).Take(pageSize).AsEnumerable();
                
            }
            catch (Exception)
            {
                throw;
            }
        }

        public T GetById(Guid Id)
        {
            return _entities.SingleOrDefault(s => s.Id == Id);
        }

        public async Task<T> Update(T _object)
        {
            try
            {
                var obj = _CarRentalDBContext.Update(_object);
                if (obj != null) await _CarRentalDBContext.SaveChangesAsync();
                return _object;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public void Delete(Guid Id)
        {
            try
            {
                if (Id != null)
                {
                    _entities.Remove(_entities.SingleOrDefault(x => x.Id == Id));
                }
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}
