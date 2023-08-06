using DataAccessLayer.Models;

namespace CarRentalSystemAPI.Dtos
{
    public class CarListDto
    {
        public int pageNumber { get; set; }
        public int pageSize { get; set; }
        public List<CarDto> results { get; set; }

     /*   public CarListDto(int pageNumber, int pageSize, List<CarDto> results)
        {
            this.pageNumber = pageNumber;
            this.pageSize = pageSize;
            this.results = results;
        }*/
    }
}
