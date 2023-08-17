using BusinessAccessLayer.Services.Interfaces;
using DataAccessLayer.Interfaces;
using DataAccessLayer.Models;

namespace BusinessAccessLayer.Services
{
    public class UsersService : IUsersService
    {
        public IUnitOfWork _unitOfWork;

        public UsersService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        public async Task<bool> CreateUsers(Users users)
        {
            if (users != null)
            {
                await _unitOfWork.Users.Create(users);

                var result = _unitOfWork.Save();

                if (result > 0)
                    return true;
                else
                    return false;
            }
            return false;
        }

        public async Task<bool> DeleteUsers(Guid usersId)
        {
            var usersDetails = await _unitOfWork.Users.Get(usersId);
            if (usersDetails != null)
            {
                _unitOfWork.Users.Delete(usersDetails);
                var result = _unitOfWork.Save();

                if (result > 0)
                    return true;
                else
                    return false;
            }
            return false;
        }

        public async Task<IEnumerable<Users>> GetAllUsers()
        {
            var usersDetailsList = await _unitOfWork.Users.GetList();
            return usersDetailsList;
        }

        public async Task<Users> GetUsersById(Guid usersId)
        {
            var usersDetails = await _unitOfWork.Users.Get(usersId);
            if (usersDetails != null)
            {
                return usersDetails;
            }
            return null;
        }

        public async Task<bool> UpdateUsers(Users users)
        {
            if (users != null)
            {
                var usersItem = await _unitOfWork.Users.Get(users.Id);
                if (usersItem != null)
                {
                    _unitOfWork.Users.Update(usersItem);

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