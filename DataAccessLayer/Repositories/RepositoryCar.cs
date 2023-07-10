using DataAccessLayer.Contracts;
using DataAccessLayer.Models;
using DataAccessLayer.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Caching.Memory;

namespace DataAccessLayer.Repositories
{
    public class RepositoryCar : IRepository<Car>
    {
        private readonly IMemoryCache _memoryCache;
        private readonly CarRentalDBContext _CarRentalDBContext;
        private readonly ILogger<Car> _logger;
        public RepositoryCar(ILogger<Car> logger, CarRentalDBContext CarRentalDBContext, IMemoryCache memoryCache)
        {
            _logger = logger;
            _CarRentalDBContext = CarRentalDBContext;
            _memoryCache = memoryCache;
        }


        public  Car Create(Car Car)
        {
            try
            {
                if (Car != null)
                {
                    var obj = _CarRentalDBContext.Cars.Add(Car);
                    Console.WriteLine(Car);
                    _CarRentalDBContext.SaveChanges();
                    return obj.Entity;
                }
                else
                {
                    return null;
                }
            }
            catch (Exception)
            {
                throw;
            }
        }
        public async void Delete(Car Car)
        {
            try
            {
                if (Car != null)
                {
                    var obj = _CarRentalDBContext.Remove(Car);
                    if (obj != null)
                    {
                       await _CarRentalDBContext.SaveChangesAsync();
                    }
                }
            }
            catch (Exception)
            {
                throw;
            }
        }

        public IEnumerable<Car> GetAll()
        {
            try
            {
                var obj = _CarRentalDBContext.Cars.ToList();
                if (obj != null) return obj;
                else return null;
            }
            catch (Exception)
            {
                throw;
            }
        }
        public IEnumerable<Car> GetAll(string Type)
        {
            try
            {
                //Get(c=>c.Type.ToLower()==Type.ToLower())
                var obj = _CarRentalDBContext.Cars.Where(c => c.Type.ToLower() == Type.ToLower());
                if (obj != null) return obj;
                else return null;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public IEnumerable<Car> GetAll(int page, int pageSize)
        {
            try
            {
                int totalNumbers = page * pageSize;

                var obj = _CarRentalDBContext.Cars.Skip(totalNumbers).Take(pageSize).ToList();
                if (obj != null) return obj;
                else return null;
            }
            catch (Exception)
            {
                throw;
            }
        }
        public Car GetById(int Id)
        {
            try
            {
                if (Id != null)
                {
                    var Obj = _CarRentalDBContext.Cars.FirstOrDefault(x => x.Id == Id);
                    if (Obj != null) return Obj;
                    else return null;
                }
                else
                {
                    return null;
                }
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<Car> Update(Car Car)
        {
            try
            {
                if (Car != null)
                {
                    var obj = _CarRentalDBContext.Update(Car);
                    if (obj != null) await _CarRentalDBContext.SaveChangesAsync();
                    return obj.Entity;

                }
                else
                {
                    return null;
                }
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}
