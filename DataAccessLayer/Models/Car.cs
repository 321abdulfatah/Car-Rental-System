using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccessLayer.Models
{
    public class Car
    {
        public int Id  { get; set; }

        public int? DriverID { get; set; }        
        [ForeignKey("DriverID")]
        public virtual Driver? Driver { get; set; }

        [Required]
        [StringLength(20, MinimumLength = 3)]
        public string Type   { get; set; }
        
        [Required]
        public double EngineCapacity   { get; set; }
        
        [Required]
        public string Color { get; set; }

        [Required]
        public double DailyFare { get; set; }

        public ICollection<Customer>? Customers { get; set; }



    }
}
