using DataAccessLayer.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace BusinessAccessLayer.Data.Config
{
    public class CustomerEntityTypeConfiguration : IEntityTypeConfiguration<Customer>
    {
        public void Configure(EntityTypeBuilder<Customer> builder)
        {
            builder.HasKey(t => t.Id);

            builder.Property(b => b.Name).HasMaxLength(20).IsRequired();
            
            builder.Property(b => b.Email).HasMaxLength(50).IsRequired();

            builder.Property(b => b.Address).HasMaxLength(50).IsRequired();

            builder.Property(b => b.Age).IsRequired();

            builder.Property(b => b.Gender).HasMaxLength(6).IsRequired();

            builder.Property(b => b.Phone).IsRequired();

        }
    }
}
