using System;
using Microsoft.EntityFrameworkCore;
using Landery.Models;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using System.Collections.Generic;

namespace Landery.Infrastructure
{
    public class Context : DbContext
    {

        public DbSet<User> Users { get; set; }
        public DbSet<Property> Properties { get; set; }
        public DbSet<PropertyImage> Images { get; set; }
        public DbSet<Favorite> Favorites { get; set; }
        public DbSet<Booking> Bookings { get; set; }


        public Context(DbContextOptions<Context> options) : base(options)
        {
            Database.EnsureCreated();

        }

        public Context()
        {
            Database.EnsureCreated();
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                optionsBuilder.UseNpgsql("Host=localhost;Port=5432;Username=postgres;Password=postgres;Database=landery-dev;");
            }
        }
    }
}
