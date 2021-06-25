using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using HotelListing.Configurations.Entities;

namespace HotelListing.Data
{
  
    public class DatabaseContext : IdentityDbContext<ApiUser>
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
            base.OnModelCreating(builder);

            builder.ApplyConfiguration(new CountryConfiguration());
            builder.ApplyConfiguration(new HotelConfiguration());

            // Creating the role configuration. 
            builder.ApplyConfiguration(new RoleConfiguration());

        }
    }
}
