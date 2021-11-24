using System;
using System.Linq;
using CarBookingApp.Core.DataServices;

using CarBookingApp.Core.Enums;

using CarBookingApp.Domain;
using CarBookingApp.Domain.Models;
using RentCarApp.Core.Models;

namespace RentCarApp.Core.Services
{
    public class CarBookingRequestProcessor : ICarBookingRequestProcessor
    {
        private readonly ICarBookingService _carBookingService;


        public CarBookingRequestProcessor(ICarBookingService carBookingService)
        {
            _carBookingService = carBookingService;
        }



        public CarBookingResponse BookCar(CarBookingRequest bookingRequest)
        {

            if (bookingRequest is null)
            {
                throw new ArgumentNullException();
            }

            var availableCars = _carBookingService.GetAvaibleCars(bookingRequest.Date);
            var result = CreateCarBookingObject<CarBookingResponse>(bookingRequest);

            if (availableCars.Any())
            {
                var car = availableCars.First();
                var carBooking = CreateCarBookingObject<CarBooking>(bookingRequest);
                carBooking.Id = car.Id;


                _carBookingService.Save(carBooking);
                result.Flag = BookingResutFlag.Success;
                result.CarBookingId = carBooking.Id;
            }
            else
            {
                result.Flag = BookingResutFlag.Failure;
            }



            return result;


        }

        private static TCarBooking CreateCarBookingObject<TCarBooking>(CarBookingRequest bookingRequest) where TCarBooking
            : CarBookingBase, new()
        {
            return new TCarBooking
            {

                FullName = bookingRequest.FullName,
                Date = bookingRequest.Date,
                Email = bookingRequest.Email
            };
        }
    }
}