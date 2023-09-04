using DataAccessLayer.Enums;
using DataAccessLayer.Models;

namespace BusinessAccessLayer.Services.Interfaces
{
    public interface IAuthService
    {
        Task<bool> Registeration(RegistrationModel model, string role);
        Task<string> Login(LoginModel model);
    }
}
