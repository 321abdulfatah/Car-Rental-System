using DataAccessLayer.Models;
using DataAccessLayer.Interfaces;
using BusinessAccessLayer.Data;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using DataAccessLayer.Common.Models;
using Abp.Linq.Extensions;
using Abp.Collections.Extensions;
using Abp.Domain.Entities;

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

        public async Task<T> Create(T entity)
        {

            if (entity != null)
            {
                await _entities.AddAsync(entity);
            }

            else
                throw new ArgumentNullException("entity");

            return entity;
        }

        public async Task<IEnumerable<T>> GetList()
        {
            return await _entities.ToListAsync();
        }

        public async Task<T> Get(Guid id)
        {
            var entity = await _entities.FindAsync(id);
            if (entity == null)
            {
                throw new EntityNotFoundException($"Entity with ID {id} not found.");
            }
            return entity;
        }

        public async Task Update(T entity)
        {
            _CarRentalDBContext.Entry(entity).State = EntityState.Modified;

            //_entities.Update(entity);
        }

        public async Task Delete(Guid id)
        {
            var entity = await _entities.FindAsync(id);

            if (entity != null)
            {
                _entities.Remove(entity);
            }
        }

    }
}