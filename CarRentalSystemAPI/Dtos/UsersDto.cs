
using Swashbuckle.AspNetCore.Annotations;

namespace CarRentalSystemAPI.Dtos
{
    public class UsersDto
    {
        public string Name { get; set; }
        public string Password { get; set; }

        [SwaggerSchema(ReadOnly = true)]
        public string? ErrorMessage { get; set; } = "";

    }
}
