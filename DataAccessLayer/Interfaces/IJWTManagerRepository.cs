using DataAccessLayer.Models;

namespace DataAccessLayer.Interfaces
{
    public interface IJWTManagerRepository
    {
        Tokens Authenticate(Users users);
    }
}
