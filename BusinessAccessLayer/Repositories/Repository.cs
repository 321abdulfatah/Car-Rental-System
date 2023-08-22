using DataAccessLayer.Models;
using DataAccessLayer.Interfaces;
using BusinessAccessLayer.Data;
using Microsoft.EntityFrameworkCore;
using Abp.Domain.Entities;
using System.Linq.Expressions;

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

        public async Task<T> CreateAsync(T entity)
        {

            if (entity != null)
            {
                await _entities.AddAsync(entity);
            }

            else
                throw new ArgumentNullException("entity");

            return entity;
        }

        public async Task<IEnumerable<T>> GetAllAsync()
        {
            return await _entities.ToListAsync();
        }

        public async Task<IEnumerable<T>> GetAllAsync(IEnumerable<Expression<Func<T, object>>> includeExpressions)
        {
            var entity =  _entities.AsQueryable();
            
            foreach (var includeExpression in includeExpressions)
            {
                entity = entity.Include(includeExpression);
            }

            return await entity.ToListAsync();
        }
        public async Task<T> GetAsync(Guid id)
        {
            var entity = await _entities.FindAsync(id);
            if (entity == null)
            {
                throw new EntityNotFoundException($"Entity with ID {id} not found.");
            }
            return entity;
        }
        public async Task<T> GetAsync(Guid id, IEnumerable<Expression<Func<T, object>>> includeExpressions)
        {
            var entity = _entities.Where(e => e.Id == id);

            foreach (var includeExpression in includeExpressions)
            {
                entity = entity.Include(includeExpression);
            }
            if (entity == null)
            {
                throw new EntityNotFoundException($"Entity with ID {id} not found.");
            }
            return await entity.SingleOrDefaultAsync();
        }
            public async Task UpdateAsync(T entity)
        {
            _CarRentalDBContext.Entry(entity).State = EntityState.Modified;

            //_entities.Update(entity);
        }

        public async Task DeleteAsync(Guid id)
        {
            var entity = await _entities.FindAsync(id);

            if (entity != null)
            {
                _entities.Remove(entity);
            }
        }
    }
}