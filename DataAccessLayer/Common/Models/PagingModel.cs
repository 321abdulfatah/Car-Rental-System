using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccessLayer.Common.Models
{
    public class PagingModel<T> where T : class
    {
        public int currentPage { get; set; } 
        public int rowsPerPage { get; set; }
        public string sortOrder { get; set; }
        public string orderBy { get; set; }
        
    }
}
