using System;
using System.Collections.Generic;
using System.Linq;
using CarBookingApp.Core.DataServices;
using CarBookingApp.Domain;

namespace CarBookingApp.Persistance.Repositories
{
    public class CarBookingService  : ICarBookingService
    {
        private readonly CarBookingAppDbContext _context;

        public CarBookingService(CarBookingAppDbContext context)
        {
            _context = context;
        }

        public IEnumerable<Car> GetAvaibleCars(DateTime date)
        {

            return _context.Cars.Where(q => q.CarBookings.Any(x => x.Date == date) == false);
        }

        public void Save(CarBooking carBooking)
        {
            _context.Add(carBooking);
            _context.SaveChanges();
        }
    }
}
