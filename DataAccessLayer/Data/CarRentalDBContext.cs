using DataAccessLayer.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccessLayer.Data
{
    public class CarRentalDBContext: DbContext
    {
        public CarRentalDBContext(DbContextOptions<CarRentalDBContext> options) : base(options)
        {
        }

        public DbSet<Car> Cars { get; set; }

    }
}
