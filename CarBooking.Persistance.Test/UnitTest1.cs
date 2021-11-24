using System;
using Xunit;
using Microsoft.EntityFrameworkCore;
using CarBookingApp.Persistance;
using CarBookingApp.Domain;
using CarBookingApp.Persistance.Repositories;
using System.Linq;

namespace CarBooking.Persistance.Test
{
    public class CarBookingServiceTest
    {
        [Fact]
        public void Should_Return_Avaible_Rooms()
        {
            //Arange

            var date = new DateTime(2021, 06, 09);
            var options = new DbContextOptionsBuilder<CarBookingAppDbContext>().UseInMemoryDatabase("CarInMemory").Options;

            using var context = new CarBookingAppDbContext(options);
            context.Add(new Car { Id = 1, Name = "Car 1" });
            context.Add(new Car { Id = 2, Name = "Car 2" });
            context.Add(new Car { Id = 3, Name = "Car 3" });

            context.Add(new CarBookingApp.Domain.CarBooking { CarId = 1, Date = date });
            context.Add(new CarBookingApp.Domain.CarBooking { CarId = 2, Date = date.AddDays(-1) });

            context.SaveChanges();

            var carBookingService = new CarBookingService(context);

            //ACT
            var availableCars = carBookingService.GetAvaibleCars(date);

            //assert
            Assert.Equal(2, availableCars.Count());
            Assert.Contains(availableCars, q => q.Id == 2);

        }

        [Fact]
        public void Should_Save_Room_Booking()
        {
            var options = new DbContextOptionsBuilder<CarBookingAppDbContext>().UseInMemoryDatabase("CarInMemorysave").Options;

            var carBooking = new CarBookingApp.Domain.CarBooking { CarId = 1, Date = new DateTime(2021, 06, 09) };

             using var context = new CarBookingAppDbContext(options);
            var carBookingService = new CarBookingService(context);
            carBookingService.Save(carBooking);

            var bookings = context.CarBookings.ToList();
            var booking = Assert.Single(bookings);

            Assert.Equal(carBooking.Date, booking.Date);

        }
    }
}
