using DataAccessLayer.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DataAccessLayer.Data
{
    public class CarEntityTypeConfiguration : IEntityTypeConfiguration<Car>
    {
        public void Configure(EntityTypeBuilder<Car> builder)
        {
            builder.HasKey(t => t.Id);
            
            builder.Property(b => b.Type).IsRequired();
            builder.Property(b => b.Type).HasMaxLength(20).IsRequired();

            builder.Property(b => b.EngineCapacity).IsRequired();
            
            builder.Property(b => b.Color).IsRequired();
            
            builder.Property(b => b.DailyFare).IsRequired();

            // builder.HasOne<Driver>(d => d.Driver)
            //.WithOne(c => c.Car)
            //.HasForeignKey<Driver>(c => c.Id).OnDelete(DeleteBehavior.Cascade);

        }
    }
}
