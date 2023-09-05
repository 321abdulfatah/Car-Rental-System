using System.ComponentModel.DataAnnotations;

namespace DataAccessLayer.Models
{
    public class LoginModel
    {
        public string Username { get; set; }

        public string Password { get; set; }

        public string Token { get; set; }
    }
}
