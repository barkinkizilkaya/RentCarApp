using System;
using CarBookingApp.Domain;
using Microsoft.EntityFrameworkCore;

namespace CarBookingApp.Persistance
{
    public class CarBookingAppDbContext :DbContext
    {
        public CarBookingAppDbContext(DbContextOptions<CarBookingAppDbContext> options) : base(options)
        {

        }

        public DbSet<Car> Cars { get; set; }

        public DbSet<CarBooking> CarBookings { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Car>().HasData(
                new Car { Id = 1, Name = "Car 1" },
                new Car { Id = 2, Name = "Car 2" },
                new Car { Id = 3, Name = "Car 3" }
                );
        }


    }
}
