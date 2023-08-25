using Abp.Domain.Entities;
using BusinessAccessLayer.Services.Interfaces;
using DataAccessLayer.Common.Models;
using DataAccessLayer.Interfaces;
using DataAccessLayer.Models;
using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;


namespace BusinessAccessLayer.Services
{
    public class UsersService : IUsersService
    {
        public IUnitOfWork _unitOfWork;

        public UsersService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        public async Task<bool> CreateUsersAsync(Users user)
        {
            if (user != null)
            {
                await _unitOfWork.Users.CreateAsync(user);

                var result = _unitOfWork.Save();

                if (result > 0)
                    return true;
                else
                    return false;
            }
            return false;
        }

        public async Task<bool> DeleteUsersAsync(Guid userId)
        {
            var userDetails = await _unitOfWork.Users.GetAsync(userId);
            if (userDetails != null)
            {
                await _unitOfWork.Users.DeleteAsync(userId);
                var result = _unitOfWork.Save();

                if (result > 0)
                    return true;
                else
                    return false;
            }
            return false;
        }

        public async Task<Users> GetUsersByIdAsync(Guid userId)
        {
            var userDetails = await _unitOfWork.Users.GetAsync(userId);
            if (userDetails == null)
            {
                throw new EntityNotFoundException($"User with ID {userId} not found.");
            }
            return userDetails;
        }

        public async Task<bool> UpdateUsersAsync(Users user)
        {
            if (user != null)
            {
                await _unitOfWork.Users.UpdateAsync(user);

                var result = _unitOfWork.Save();

                if (result > 0)
                    return true;
                else
                    return false;
                
            }
            return false;
        }
        public async Task<PaginatedResult<Users>> GetListUsersAsync(string searchTerm, string sortBy, int pageIndex, int pageSize)
        {
            Expression<Func<Users, bool>> filter = user => true;

            if (!string.IsNullOrEmpty(searchTerm))
                filter = user => user.Name.Contains(searchTerm);

            var query = _unitOfWork.Users.GetAll();

            // Apply filter
            query = query.Where(filter);

            // Calculate total count
            var totalCount = await query.CountAsync();

            if (!string.IsNullOrEmpty(sortBy))
            {
                // Split the sortBy string into column name and sorting direction
                var sortByParts = sortBy.Split(' ');
                string columnName = sortByParts[0];
                string sortingDirection = sortByParts.Length > 1 ? sortByParts[1] : "asc"; // Default to ascending

                // Determine the sorting order
                bool isDescending = sortingDirection.Equals("desc", StringComparison.OrdinalIgnoreCase);

                switch (columnName.ToLower())
                {
                    case "name":
                        query = isDescending ? query.OrderByDescending(c => c.Name) : query.OrderBy(c => c.Name);
                        break;
                    default:
                        query = query.OrderBy(c => c.Id);
                        break;
                }
            }

            var pagedUsers = await query.Skip((pageIndex - 1) * pageSize).Take(pageSize).ToListAsync();

            return new PaginatedResult<Users>
            {
                Data = pagedUsers,
                TotalCount = totalCount
            };
        }
    }
}