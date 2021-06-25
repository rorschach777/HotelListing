using System;
using HotelListing.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace HotelListing.Configurations.Entities
{
    public class HotelConfiguration : IEntityTypeConfiguration<Hotel>
    {
        public void Configure(EntityTypeBuilder<Hotel> builder)
        {
            builder.HasData(
                 new Hotel
                 {
                     Id = 1,
                     Name = "Sandals Resort and Spa",
                     Address = "1234 Main st.,",
                     CountryId = 1,
                     Rating = 4.5
                 },
                new Hotel
                {
                    Id = 2,
                    Name = "Comfort Suites",
                    Address = "9876 comfort st.,",
                    CountryId = 3,
                    Rating = 4.2
                },
                new Hotel
                {
                    Id = 3,
                    Name = "Atlantis",
                    Address = "876 Atlantis way.,",
                    CountryId = 2,
                    Rating = 4.8
                }
            );
        }
    }
}
