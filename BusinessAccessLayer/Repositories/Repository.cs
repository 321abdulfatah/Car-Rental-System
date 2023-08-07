using DataAccessLayer.Models;
using DataAccessLayer.Interfaces;
using DataAccessLayer.Data;
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

        public async Task<T> Create(T _object)
        {
            
            if (_object != null)
            {
                _entities.Add(_object); 
                await _CarRentalDBContext.SaveChangesAsync();
            }

            else
                throw new ArgumentNullException("entity");

            return _object;
        }
        
        public PaginatedResult<T> GetList(string Search, string Column, string SortOrder, string OrderBy, int PageIndex, int PageSize)
        {
            try
            {

                var query = _entities.AsQueryable();

                // 1- Filtering by using where, . . .. 
                var filterQuery = String.IsNullOrWhiteSpace(Column) ? query : String.IsNullOrWhiteSpace(Search) ? query : query = query.Where($"{Column}.Contains(@0)", Search);

                // 2- Get count of query filtered
                var totalCount = filterQuery.Count();

                //3- Apply sorting
                var sortOrderTerm = (SortOrder != "asc") ? " descending" : string.Empty;

                var finalQuery = String.IsNullOrWhiteSpace(OrderBy) ? filterQuery : filterQuery.OrderBy(OrderBy + sortOrderTerm);
                
                //4- Apply paging
                var itemsToSkip = (PageIndex - 1) * PageSize;

                var result = finalQuery.Skip(itemsToSkip).Take(PageSize).ToList();

                //5- Map to output (should contains count and list paginated)

                PaginatedResult<T> res = new PaginatedResult<T>();

                res.TotalRows = totalCount;
                res.TotalPages = (int)Math.Ceiling((double)totalCount / PageSize);
                res.Items = result;

                return res;
                
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<T> Get(Guid Id)
        {
            var entity = await _entities.FirstOrDefaultAsync(s => s.Id == Id);
            if (entity == null)
            {
                throw new EntityNotFoundException($"Entity with ID {Id} not found.");
            }
            return entity;
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

        public async Task<T> Delete(Guid Id)
        {
            try
            {
                var obj = _entities.SingleOrDefault(x => x.Id == Id);
                     
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
