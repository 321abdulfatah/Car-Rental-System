using DataAccessLayer.Models;


namespace BusinessAccessLayer.Services.Interfaces
{
    public interface IRentalService
    {
        Task<bool> CreateRental(Rental rental);

        Task<IEnumerable<Rental>> GetAllRental();

        Task<Rental> GetRentalById(Guid rentalId);

        Task<bool> UpdateRental(Rental rental);

        Task<bool> DeleteRental(Guid rentalId);
    }
}
