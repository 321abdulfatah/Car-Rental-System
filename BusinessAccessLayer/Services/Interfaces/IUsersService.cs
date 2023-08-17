using DataAccessLayer.Models;


namespace BusinessAccessLayer.Services.Interfaces
{
    public interface IUsersService
    {
        Task<bool> CreateUsers(Users users);

        Task<IEnumerable<Users>> GetAllUsers();

        Task<Users> GetUsersById(Guid usersId);

        Task<bool> UpdateUsers(Users users);

        Task<bool> DeleteUsers(Guid usersId);
    }
}
