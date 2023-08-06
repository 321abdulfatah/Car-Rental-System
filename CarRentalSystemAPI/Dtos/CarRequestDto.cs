namespace CarRentalSystemAPI.Dtos
{
    public class CarRequestDto
    {
        public int PageNumber { get; set; }
        public int PageSize { get; set; }
        public CarRequestDto()
        {
            this.PageNumber = 1;
            this.PageSize = 10;
        }
        public CarRequestDto(int pageNumber, int pageSize)
        {
            this.PageNumber = pageNumber < 1 ? 1 : pageNumber;
            this.PageSize = pageSize > 10 ? 10 : pageSize;
        }

    }
}
