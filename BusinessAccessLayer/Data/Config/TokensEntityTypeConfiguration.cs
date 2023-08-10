using DataAccessLayer.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace BusinessAccessLayer.Data.Config
{
    public class TokensEntityTypeConfiguration : IEntityTypeConfiguration<Tokens>
    {
        public void Configure(EntityTypeBuilder<Tokens> builder)
        {

            builder.Property(b => b.Token).IsRequired();

            builder.Property(b => b.RefreshToken).IsRequired();
        }
    }
}
