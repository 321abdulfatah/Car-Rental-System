﻿using DataAccessLayer.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace BusinessAccessLayer.Data.Config
{
    public class RentalEntityTypeConfiguration : IEntityTypeConfiguration<Rental>
    {
        public void Configure(EntityTypeBuilder<Rental> builder)
        {
            builder.HasKey(t => t.Id);

            builder.Property(b => b.CarId).IsRequired();

            builder.Property(b => b.CustomerId).IsRequired();

            builder.Property(b => b.Rent).IsRequired();

            builder.Property(b => b.StatusRent).IsRequired();

            builder.Property(b => b.StartDateRent).IsRequired();

            builder.Property(b => b.RentTerm).IsRequired();
        }
    }
}
