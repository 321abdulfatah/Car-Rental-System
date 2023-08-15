using DataAccessLayer.Models;
using DataAccessLayer.Interfaces;
using BusinessAccessLayer.Data;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Linq.Dynamic.Core;
using DataAccessLayer.Common.Models;
using Abp.Domain.Entities;
using Abp.Linq.Extensions;
using Abp.Collections.Extensions;

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

        public async Task<T> Create(T obj)
        {

            if (obj != null)
            {
                _entities.Add(obj);
                await _CarRentalDBContext.SaveChangesAsync();
            }

            else
                throw new ArgumentNullException("entity");

            return obj;
        }

        public PaginatedResult<T> GetList(string search, string column, string sortOrder, string orderBy, int pageIndex, int pageSize)
        {
            try
            {

                var query = _entities.AsQueryable();

                // 1- Filtering by using where, . . .. 
                var filterQuery = String.IsNullOrWhiteSpace(column) ? query : String.IsNullOrWhiteSpace(search) ? query : query = query.Where($"{column}.Contains(@0)", search);

                // 2- Get count of query filtered
                var totalCount = filterQuery.Count();

                //3- Apply sorting
                var sortOrderTerm = (sortOrder != "asc") ? " descending" : string.Empty;

                var finalQuery = String.IsNullOrWhiteSpace(orderBy) ? filterQuery : filterQuery.OrderBy(orderBy + sortOrderTerm);

                //4- Apply paging
                var itemsToSkip = (pageIndex - 1) * pageSize;

                var result = finalQuery.Skip(itemsToSkip).Take(pageSize).ToList();

                //5- Map to output (should contains count and list paginated)

                PaginatedResult<T> res = new PaginatedResult<T>();

                res.TotalRows = totalCount;
                res.TotalPages = (int)Math.Ceiling((double)totalCount / pageSize);
                res.Items = result;

                return res;

            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<T> Get(Guid id)
        {
            var entity = await _entities.FirstOrDefaultAsync(s => s.Id == id);
            if (entity == null)
            {
                throw new EntityNotFoundException($"Entity with ID {id} not found.");
            }
            return entity;
        }

        public async Task<T> Update(T obj)
        {
            try
            {
                var entity = _entities.Update(obj);
                if (entity != null) await _CarRentalDBContext.SaveChangesAsync();
                return obj;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<T> Delete(Guid id)
        {
            try
            {
                var obj = _entities.SingleOrDefault(x => x.Id == id);

                _entities.Remove(obj);
                await _CarRentalDBContext.SaveChangesAsync();

                return obj;

            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}