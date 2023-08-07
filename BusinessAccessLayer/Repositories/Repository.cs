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

        public async Task<T> Create(T _object)
        {
            if (_object == null)
            {
                throw new ArgumentNullException("entity");
            }
            _entities.Add(_object);
            _CarRentalDBContext.SaveChanges();

            return _object;
        }
        
        public IQueryable<T> GetList()
        {
            try
            {

                return _entities.AsQueryable();
                
            }
            catch (Exception)
            {
                throw;
            }
        }

        public T Get(Guid Id)
        {
            return _entities.SingleOrDefault(s => s.Id == Id);
        }

        public async Task<T> Update(T _object)
        {
            try
            {
                var obj = _entities.Update(_object);
                if (obj != null) await _CarRentalDBContext.SaveChangesAsync();
                return _object;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public T Delete(Guid Id)
        {
            try
            {
                var obj = _entities.SingleOrDefault(x => x.Id == Id);
                     
                _entities.Remove(obj);
                _CarRentalDBContext.SaveChangesAsync();

                return obj;
                
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}
