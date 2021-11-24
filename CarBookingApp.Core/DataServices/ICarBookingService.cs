using System;
using System.Collections.Generic;

using CarBookingApp.Domain;

namespace CarBookingApp.Core.DataServices
{
    public interface ICarBookingService
    {
        void Save(CarBooking carBooking);

        IEnumerable<Car> GetAvaibleCars(DateTime date);
    }
}
