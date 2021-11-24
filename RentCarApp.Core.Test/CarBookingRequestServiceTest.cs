using System;
using System.Collections.Generic;
using System.Linq;
using CarBookingApp.Core.DataServices;

using CarBookingApp.Core.Enums;
using CarBookingApp.Domain;
using Moq;
using RentCarApp.Core.Models;
using RentCarApp.Core.Services;
using Shouldly;
using Xunit;

namespace RentCarApp.Core
{
    public class CarBookingRequestServicesTest
    {
        private CarBookingRequestProcessor _service;
        private CarBookingRequest _bookingRequest;
        private Mock<ICarBookingService> _carBookingServiceMock;
        private List<Car> _availableCars;

        public CarBookingRequestServicesTest()
        {
           
            //Arrange
            _bookingRequest = new CarBookingRequest
            {
                FullName = "Test Name-Surname",
                Email = "test@testmail.com",
                Date = new DateTime(2021, 11, 10)

            };

            _availableCars = new List<Car>() { new Car() { Id = 1 } };
            
            _carBookingServiceMock = new Mock<ICarBookingService>();
            _carBookingServiceMock.Setup(q => q.GetAvaibleCars(_bookingRequest.Date)).
                Returns(_availableCars);

            _service = new CarBookingRequestProcessor(_carBookingServiceMock.Object);


        }
        [Fact]
        public void Should_Return_Car_Booking_Response_With_Request_Values()
        {

          
            //Act
            CarBookingResponse result = _service.BookCar(_bookingRequest);

            //Assert
            Assert.NotNull(result);
            Assert.Equal(_bookingRequest.FullName, result.FullName);
            Assert.Equal(_bookingRequest.Email, result.Email);
            Assert.Equal(_bookingRequest.Date, result.Date);

            result.ShouldNotBeNull();
            result.FullName.ShouldBe(_bookingRequest.FullName);
        }


        [Fact]
        public void Should_Throw_Exception_For_Null_Request()
        {
           
            Should.Throw<ArgumentNullException>(() => _service.BookCar(null));
        }

        [Fact]
        public void Should_Save_Car_Boking_Request()
        {
            CarBooking savedBooking = null;

            _carBookingServiceMock.Setup(q => q.Save(It.IsAny<CarBooking>())).Callback<CarBooking>(booking => {

                savedBooking = booking;

            });

            _service.BookCar(_bookingRequest);

            _carBookingServiceMock.Verify(q => q.Save(It.IsAny<CarBooking>()), Times.Once);

            savedBooking.ShouldNotBeNull();
            savedBooking.FullName.ShouldBe(_bookingRequest.FullName);
            savedBooking.Email.ShouldBe(_bookingRequest.Email);
            savedBooking.Date.ShouldBe(_bookingRequest.Date);
            savedBooking.Id.ShouldBe(_availableCars.First().Id);
        }

        [Fact]
        public void Should_Not_Save_Car_If_No_Available()
        {
            _availableCars.Clear();

            _service.BookCar(_bookingRequest);

            _carBookingServiceMock.Verify(q => q.Save(It.IsAny<CarBooking>()), Times.Never);

        }

        [Theory]
        [InlineData(BookingResutFlag.Failure,false)]
        [InlineData(BookingResutFlag.Success, true)]
        public void Should_Return_SuccessOrFailure_Flag_In_Result(BookingResutFlag bookingSuccessFlag, bool isAvailable)
        {
            if(!isAvailable)
            {
                _availableCars.Clear();

            }

            var result = _service.BookCar(_bookingRequest);
            bookingSuccessFlag.ShouldBe(result.Flag);
        }

        [Theory]
        [InlineData(1,true)]
        [InlineData(null,false)]
        public void Should_Return_CarBookingId_In_Result(int? carBookingId,bool isAvailable)
        {
            if (!isAvailable)
            {
                _availableCars.Clear();

            }
            else
            {

                _carBookingServiceMock.Setup(q => q.Save(It.IsAny<CarBooking>())).Callback<CarBooking>(booking => {

                    booking.Id = carBookingId.Value;

                });
            }

            var result = _service.BookCar(_bookingRequest);
            result.CarBookingId.ShouldBe(carBookingId);
        }

    }
}
