﻿using DataAccessLayer.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace BusinessAccessLayer.Data.Config
{
    public class DriverEntityTypeConfiguration : IEntityTypeConfiguration<Driver>
    {
        public void Configure(EntityTypeBuilder<Driver> builder)
        {
            builder.HasKey(t => t.Id);

            builder.Property(b => b.Name).HasMaxLength(20).IsRequired();

            builder.Property(b => b.Email).HasMaxLength(50).IsRequired();

            builder.Property(b => b.Address).HasMaxLength(50).IsRequired();

            builder.Property(b => b.Age).IsRequired();

            builder.Property(b => b.Gender).HasMaxLength(6).IsRequired();

            builder.Property(b => b.Phone).IsRequired();

            builder.Property(b => b.Salary).IsRequired();
            
            builder.Property(b => b.IsAvailable).IsRequired();


            builder.HasOne<Driver>()
            .WithOne(d => d.ReplacmentDriver)
            .HasForeignKey<Driver>(c => c.ReplacmentDriverId).OnDelete(DeleteBehavior.ClientSetNull);
        }
    }
}
