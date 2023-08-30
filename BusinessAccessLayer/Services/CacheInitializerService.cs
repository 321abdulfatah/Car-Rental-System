using BusinessAccessLayer.Services.Interfaces;
using DataAccessLayer.Interfaces;
using DataAccessLayer.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;
using System.Linq.Expressions;


namespace BusinessAccessLayer.Services
{
    public class CacheInitializerService : ICacheInitializerService

    {
        private readonly IMemoryCache _memoryCache;
        private readonly IUnitOfWork _unitOfWork;

        public CacheInitializerService(IMemoryCache memoryCache, IUnitOfWork unitOfWork)
        {
            _memoryCache = memoryCache;
            _unitOfWork = unitOfWork;
        }

        public async Task InitializeCacheAsync()
        {
            var includeExpressions = new Expression<Func<Car, object>>[]
            {
                c => c.Driver,
            };

            var cars = await _unitOfWork.Cars.GetAll(includeExpressions).ToListAsync();

            _memoryCache.Set(CacheKeys.Cars, cars);
        }
        
    }
}
