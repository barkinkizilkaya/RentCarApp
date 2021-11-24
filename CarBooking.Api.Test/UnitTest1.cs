using System;
using Xunit;
using System.Linq;
using Moq;
using CarBooking.WebApi;
using Castle.Core.Logging;
using CarBooking.WebApi.Controllers;
using Microsoft.Extensions.Logging;
using Shouldly;
using RentCarApp.Core.Services;
using RentCarApp.Core.Models;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace CarBooking.Api.Test
{
    public class CarBookingApiTest
    {
        private Mock<ICarBookingRequestProcessor> _roomBookingService;
        private CarApiController _controller;
        private CarBookingRequest _request;
        private CarBookingResponse _response;

        public CarBookingApiTest()
        {
            _roomBookingService = new Mock<ICarBookingRequestProcessor>();
            _controller = new CarApiController(_roomBookingService.Object);
            _request = new CarBookingRequest();
            _response = new CarBookingResponse();

            _roomBookingService.Setup(x => x.BookCar(_request)).Returns(_response);
        }

        [Fact]
        public void ShouldReturnForecastResult()
        {
            var loggerMoq = new Mock<ILogger<WeatherForecastController>>();
            var controller = new WeatherForecastController(loggerMoq.Object);

            var result = controller.Get();

            result.Count().ShouldBeGreaterThan(1);
            result.ShouldNotBeNull();
        }

        [Theory]
        [InlineData(1,true,typeof(OkObjectResult))]
        [InlineData(0,false,typeof(BadRequestObjectResult))]
        public async Task Should_Call_Booking_Method_When_Valid(int expectedMethodCalls, bool isModelValid, Type exceptedActionResultType)
        {
            if(!isModelValid)
            {
                _controller.ModelState.AddModelError("Key", "ErrorMessage");

            }

            //act
            var result = await _controller.BookCar(_request);

            //Assert
            result.ShouldBeOfType(exceptedActionResultType);
            _roomBookingService.Verify(x => x.BookCar(_request), Times.Exactly(expectedMethodCalls));
        }
    }
}
