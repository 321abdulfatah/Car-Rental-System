namespace DataAccessLayer.Interfaces
{
    public interface IUnitOfWork : IDisposable
    {
        ICarRepository Cars { get; }

        ICustomerRepository Customers { get; }

        IDriverRepository Drivers { get; }

        IRentalRepository Rentals { get; }
        int Save();
    }
}
