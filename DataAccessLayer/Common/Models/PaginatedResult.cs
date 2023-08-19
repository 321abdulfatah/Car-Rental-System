namespace DataAccessLayer.Common.Models
{
    public class PaginatedResult<T> where T : class
    {
        public List<T> Data { get; set; }
        public int TotalCount { get; set; }
    }
}
