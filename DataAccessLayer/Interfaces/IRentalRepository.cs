using DataAccessLayer.Models;


namespace DataAccessLayer.Interfaces
{
    public interface IRentalRepository : IRepository<Rental>
    {
        Task<bool> IsCarRentedAsync(Guid carId);

    }
}
