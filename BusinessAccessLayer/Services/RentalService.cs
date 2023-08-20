using BusinessAccessLayer.Services.Interfaces;
using DataAccessLayer.Common.Models;
using DataAccessLayer.Interfaces;
using DataAccessLayer.Models;
using System.Linq.Expressions;

namespace BusinessAccessLayer.Services
{
    public class RentalService : IRentalService
    {
        public IUnitOfWork _unitOfWork;

        public RentalService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        public async Task<bool> CreateRentalAsync(Rental rental)
        {
            if (rental != null)
            {
                await _unitOfWork.Rentals.CreateAsync(rental);

                var result = _unitOfWork.Save();

                if (result > 0)
                    return true;
                else
                    return false;
            }
            return false;
        }

        public async Task<bool> DeleteRentalAsync(Guid rentalId)
        {
            var rentalDetails = await _unitOfWork.Rentals.GetAsync(rentalId);
            if (rentalDetails != null)
            {
                _unitOfWork.Rentals.DeleteAsync(rentalId);
                var result = _unitOfWork.Save();

                if (result > 0)
                    return true;
                else
                    return false;
            }
            return false;
        }

        public async Task<IEnumerable<Rental>> GetAllRentalAsync()
        {
            var rentalDetailsList = await _unitOfWork.Rentals.GetAllAsync();
            return rentalDetailsList;
        }

        public async Task<Rental> GetRentalByIdAsync(Guid rentalId)
        {
            var rentalDetails = await _unitOfWork.Rentals.GetAsync(rentalId);
            if (rentalDetails != null)
            {
                return rentalDetails;
            }
            return null;
        }

        public async Task<bool> UpdateRentalAsync(Rental rental)
        {
            if (rental != null)
            {
                var rentalItem = await _unitOfWork.Rentals.GetAsync(rental.Id);
                if (rentalItem != null)
                {
                    _unitOfWork.Rentals.UpdateAsync(rentalItem);

                    var result = _unitOfWork.Save();

                    if (result > 0)
                        return true;
                    else
                        return false;
                }
            }
            return false;
        }
        public async Task<PaginatedResult<Rental>> GetListRentalsAsync(Expression<Func<Rental, bool>> filter, string sortBy, bool isAscending, int pageIndex, int pageSize)
        {
            return await _unitOfWork.Rentals.GetListAsync(filter, sortBy, isAscending, pageIndex, pageSize);
        }
    }
}
