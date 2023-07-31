using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccessLayer.Models
{
    public class CarRent
    {
        public int Id { get; set; }

        public int? CarID { get; set; }
        [ForeignKey("CarID")]
        public virtual Car Car { get; set; }
        
        public int? CustomerID { get; set; }
        [ForeignKey("CustomerID")]
        public virtual Customer Customer { get; set; }

        public int? DriverID { get; set; }
        [ForeignKey("DriverID")]
        public virtual Driver? Driver { get; set; }

        public DateTime StratDateRent{ get; set; }

        public int RentTerm { get; set; }
    }
}
