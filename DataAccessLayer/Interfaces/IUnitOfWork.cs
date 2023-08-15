
namespace DataAccessLayer.Interfaces
{
    public interface IUnitOfWork : IDisposable
    {
        ICarRepository Cars { get; }

        IUsersRepository Users { get; }

        ICustomerRepository Customers { get; }

        IDriverRepository Drivers { get; }

        IRentalRepository Rentals { get; }
        Task <int> SaveAsync();
    }
}
