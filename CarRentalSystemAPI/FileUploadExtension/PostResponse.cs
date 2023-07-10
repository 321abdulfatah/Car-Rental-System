using DataAccessLayer.Models;

namespace CarRentalSystemAPI.FileUploadExtension
{
    public class PostResponse : BaseResponse
    {
        public Car Post { get; set; }
    }
}
