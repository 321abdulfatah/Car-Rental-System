namespace DataAccessLayer.Common.Models
{
    public class PagingModel<T> where T : class
    {
        private int _pageIndex = 1;

        public int PageIndex
        {
            get => _pageIndex; 
            set => _pageIndex = (value < 1) ? 1 : value; 
        }

        private int _pageSize = 5;
        public int PageSize
        {
            get => _pageSize;
            set => _pageSize = value; 
        }
    }
}
