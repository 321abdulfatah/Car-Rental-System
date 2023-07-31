using DataAccessLayer.Models;
using DataAccessLayer.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.Caching.Memory;

namespace BusinessAccessLayer.Services
{
    public class ServiceCar : IServiceCar<Car>
    {

        private readonly IRepository<Car> _repository;

        private readonly IMemoryCache _memoryCache;

        public ServiceCar(IRepository<Car> repository, IMemoryCache memoryCache)
        {
            _repository = repository;
            _memoryCache = memoryCache;
        }

        //Create Method
        public Car Create(Car Car)
        {
            try
            {
                if (Car == null)
                {
                    throw new ArgumentNullException(nameof(Car));
                }
                else
                {
                    return _repository.Create(Car);
                }
            }
            catch (Exception)
            {
                throw;
            }
        }
        public void Delete(int Id)
        {
            try
            {
                if (Id != 0)
                {
                    var obj = _repository.GetAll().Where(x => x.Id == Id).FirstOrDefault();
                    _repository.Delete(obj);
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

                if (Car == null)
                {
                    throw new ArgumentNullException(nameof(Car));
                }
                else
                {
                    

                    var CarToUpdate = _repository.GetById(Car.Id);

                    if (CarToUpdate == null)
                        return null;

                    CarToUpdate.EngineCapacity = Car.EngineCapacity;
                    CarToUpdate.Type = Car.Type;
                    CarToUpdate.DailyFare = Car.DailyFare;
                    CarToUpdate.Color = Car.Color;

                    return await _repository.Update(Car);
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
                var output = _memoryCache.Get<IEnumerable<Car>>("cars");

                if (output is not null) return output;

                output =  _repository.GetAll().ToList();

                _memoryCache.Set("cars", output, TimeSpan.FromMinutes(1));

                return output;

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
                return _repository.GetAll(Type).ToList();
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
                return _repository.GetAll(page,pageSize).ToList();
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
               return  _repository.GetById(Id);
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}