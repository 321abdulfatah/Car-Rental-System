using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccessLayer.Common.Models
{
    public class PagingModel<T> where T : class
    {
        public int CurrentPage { get; set; } 
        public int TotalPages { get; set; }
        public int RowsPerPage { get; set; }
        public int TotalRows { get; set; }
        public IEnumerable<T> Results { get; set; } = Array.Empty<T>();

        
    }
}
