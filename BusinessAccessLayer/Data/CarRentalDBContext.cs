using BusinessAccessLayer.Data.Config;
using DataAccessLayer.Models;
using Microsoft.EntityFrameworkCore;

namespace BusinessAccessLayer.Data
{
    public class CarRentalDBContext: DbContext
    {
        public CarRentalDBContext(DbContextOptions<CarRentalDBContext> options) : base(options)
        {
        }

        public DbSet<Car> Cars { get; set; }

        public DbSet<Customer> Customers { get; set; }

        public DbSet<Driver> Drivers { get; set; }

        public DbSet<Rental> Rentals{ get; set; }

        public DbSet<Users> Users { get; set; }

        public DbSet<Tokens> Tokens { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            new CarEntityTypeConfiguration().Configure(modelBuilder.Entity<Car>());
            new CustomerEntityTypeConfiguration().Configure(modelBuilder.Entity<Customer>());
            new DriverEntityTypeConfiguration().Configure(modelBuilder.Entity<Driver>());
            new RentalEntityTypeConfiguration().Configure(modelBuilder.Entity<Rental>());
            new UsersEntityTypeConfiguration().Configure(modelBuilder.Entity<Users>());
            new TokensEntityTypeConfiguration().Configure(modelBuilder.Entity<Tokens>());

        }
    }
}
