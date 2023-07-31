using DataAccessLayer.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccessLayer.Models
{
    public class Rental
    {
        public Guid Id { get; set; }

        public Guid? CarID { get; set; }
        [ForeignKey("CarID")]
        public virtual Car Car { get; set; }
        
        public Guid? CustomerID { get; set; }
        [ForeignKey("CustomerID")]
        public virtual Customer Customer { get; set; }

        public Guid? DriverID { get; set; }
        [ForeignKey("DriverID")]
        public virtual Driver? Driver { get; set; }

        public int Rent { get; set; }

        public StatusRent StatusRent { get; set; }

        public DateTime StratDateRent{ get; set; }

        public int RentTerm { get; set; }
    }
}
