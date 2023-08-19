using BusinessAccessLayer.Services.Interfaces;
using DataAccessLayer.Interfaces;
using DataAccessLayer.Models;

namespace BusinessAccessLayer.Services
{
    public class RentalService : IRentalService
    {
        public IUnitOfWork _unitOfWork;

        public RentalService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        public async Task<bool> CreateRental(Rental rental)
        {
            if (rental != null)
            {
                await _unitOfWork.Rentals.Create(rental);

                var result = _unitOfWork.Save();

                if (result > 0)
                    return true;
                else
                    return false;
            }
            return false;
        }

        public async Task<bool> DeleteRental(Guid rentalId)
        {
            var rentalDetails = await _unitOfWork.Rentals.Get(rentalId);
            if (rentalDetails != null)
            {
                _unitOfWork.Rentals.Delete(rentalId);
                var result = _unitOfWork.Save();

                if (result > 0)
                    return true;
                else
                    return false;
            }
            return false;
        }

        public async Task<IEnumerable<Rental>> GetAllRental()
        {
            var rentalDetailsList = await _unitOfWork.Rentals.GetList();
            return rentalDetailsList;
        }

        public async Task<Rental> GetRentalById(Guid rentalId)
        {
            var rentalDetails = await _unitOfWork.Rentals.Get(rentalId);
            if (rentalDetails != null)
            {
                return rentalDetails;
            }
            return null;
        }

        public async Task<bool> UpdateRental(Rental rental)
        {
            if (rental != null)
            {
                var rentalItem = await _unitOfWork.Rentals.Get(rental.Id);
                if (rentalItem != null)
                {
                    _unitOfWork.Rentals.Update(rentalItem);

                    var result = _unitOfWork.Save();

                    if (result > 0)
                        return true;
                    else
                        return false;
                }
            }
            return false;
        }
    }
}
