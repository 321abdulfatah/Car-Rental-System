using DataAccessLayer.Interfaces;
using BusinessAccessLayer.Data;

namespace BusinessAccessLayer.Repositories
{
    public class UnitOfWork : IUnitOfWork
    {

        private readonly CarRentalDBContext _dbContext;
        public ICarRepository Cars { get; }
        public IUsersRepository Users { get; }
        public ICustomerRepository Customers { get; }
        public IDriverRepository Drivers { get; }
        public IRentalRepository Rentals { get; }

        public UnitOfWork(CarRentalDBContext dbContext,
                            ICarRepository carRepository,
                            IUsersRepository UserRepository,
                            ICustomerRepository CustomerRepository,
                            IDriverRepository DriverRepository,
                            IRentalRepository RentalRepository)
        {
            _dbContext = dbContext;
            Cars = carRepository;
            Users = UserRepository;
            Customers = CustomerRepository;
            Drivers = DriverRepository;
            Rentals = RentalRepository; 
        }

        public int Save()
        {
            return  _dbContext.SaveChanges();
        }

        public void Dispose()
        {
            DisposeAsync(true);
            GC.SuppressFinalize(this);
        }

        protected virtual async Task DisposeAsync(bool disposing)
        {
            if (disposing)
            {
                await _dbContext.DisposeAsync();
            }
        }
    }
}
