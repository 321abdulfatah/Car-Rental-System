using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccessLayer.Models
{
    public class CarRent
    {
        public Guid Id { get; set; }

        public Car Car { get; set; }

        public Customer Customer{ get; set; }

        public Driver? Driver { get; set; }

        public DateTime StratDateRent{ get; set; }

        public int RentTerm { get; set; }
    }
}
