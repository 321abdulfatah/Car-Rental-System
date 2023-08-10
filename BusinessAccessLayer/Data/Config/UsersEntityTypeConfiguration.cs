using DataAccessLayer.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace BusinessAccessLayer.Data.Config
{
    public class UsersEntityTypeConfiguration : IEntityTypeConfiguration<Users>
    {
        public void Configure(EntityTypeBuilder<Users> builder)
        {
           
            builder.Property(b => b.Name).IsRequired();

            builder.Property(b => b.Password).IsRequired();
        }
    }
}
