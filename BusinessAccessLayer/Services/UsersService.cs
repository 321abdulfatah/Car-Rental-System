using BusinessAccessLayer.Services.Interfaces;
using DataAccessLayer.Common.Models;
using DataAccessLayer.Interfaces;
using DataAccessLayer.Models;
using System.Linq.Expressions;

namespace BusinessAccessLayer.Services
{
    public class UsersService : IUsersService
    {
        public IUnitOfWork _unitOfWork;

        public UsersService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        public async Task<bool> CreateUsersAsync(Users users)
        {
            if (users != null)
            {
                await _unitOfWork.Users.CreateAsync(users);

                var result = _unitOfWork.Save();

                if (result > 0)
                    return true;
                else
                    return false;
            }
            return false;
        }

        public async Task<bool> DeleteUsersAsync(Guid usersId)
        {
            var usersDetails = await _unitOfWork.Users.GetAsync(usersId);
            if (usersDetails != null)
            {
                _unitOfWork.Users.DeleteAsync(usersId);
                var result = _unitOfWork.Save();

                if (result > 0)
                    return true;
                else
                    return false;
            }
            return false;
        }

        public async Task<IEnumerable<Users>> GetAllUsersAsync()
        {
            var usersDetailsList = await _unitOfWork.Users.GetAllAsync();
            return usersDetailsList;
        }

        public async Task<Users> GetUsersByIdAsync(Guid usersId)
        {
            var usersDetails = await _unitOfWork.Users.GetAsync(usersId);
            if (usersDetails != null)
            {
                return usersDetails;
            }
            return null;
        }

        public async Task<bool> UpdateUsersAsync(Users users)
        {
            if (users != null)
            {
                var usersItem = await _unitOfWork.Users.GetAsync(users.Id);
                if (usersItem != null)
                {
                    _unitOfWork.Users.UpdateAsync(usersItem);

                    var result = _unitOfWork.Save();

                    if (result > 0)
                        return true;
                    else
                        return false;
                }
            }
            return false;
        }
        public async Task<PaginatedResult<Users>> GetListUsersAsync(Expression<Func<Users, bool>> filter, string sortBy, bool isAscending, int pageIndex, int pageSize)
        {
            return await _unitOfWork.Users.GetListAsync(filter, sortBy, isAscending, pageIndex, pageSize);
        }
    }
}