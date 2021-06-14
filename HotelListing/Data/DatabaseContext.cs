using System;
using Microsoft.EntityFrameworkCore;

namespace HotelListing.Data
{
  
    public class DatabaseContext : DbContext
    {
        public DatabaseContext(DbContextOptions options): base(options)
        {
         
        }

        /// <summary>
        ///  This is the bridge between the model classes and the database.
        ///  here is where we list the database should know about. 
        /// </summary>
        ///

        public DbSet<Country> Countries { get; set; }

        public DbSet<Hotel> Hotels { get; set; }



        protected override void OnModelCreating(ModelBuilder builder)
        {
            builder.Entity<Country>().HasData(
                new Country
                {
                    Id = 1,
                    Name = "Jamaica",
                    ShortName = "JM"
                },
                new Country
                {
                    Id = 2,
                    Name = "Bahamas",
                    ShortName = "BS"
                },
                new Country
                {
                    Id = 3,
                    Name = "Cayman Islands",
                    ShortName = "CI"
                }
           );
            builder.Entity<Hotel>().HasData(
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
