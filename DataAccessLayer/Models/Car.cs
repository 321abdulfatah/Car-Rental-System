using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccessLayer.Models
{
    public class Car : BaseModel
    {
        public Guid? DriverId { get; set; }        

        public virtual Driver Driver { get; set; }

        public string Type   { get; set; }
        
        public double EngineCapacity   { get; set; }
        
        public string Color { get; set; }

        public double DailyFare { get; set; }

    }
}
