namespace DataAccessLayer.Common.Models
{
    public class PaginatedResult<T> where T : class
    {
        public int TotalPages { get; set; }
        public int TotalRows { get; set; }
        public List<T> Items { get; set; }
    }
}
