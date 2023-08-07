using DataAccessLayer.Models;

namespace CarRentalSystemAPI.Dtos
{
    public class CarListDto
    {       
        public IEnumerable<CarDto> results { get; set; }
 
    }
}
