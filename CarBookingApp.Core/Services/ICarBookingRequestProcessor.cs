using RentCarApp.Core.Models;

namespace RentCarApp.Core.Services
{
    public interface ICarBookingRequestProcessor
    {
        CarBookingResponse BookCar(CarBookingRequest bookingRequest);
    }
}